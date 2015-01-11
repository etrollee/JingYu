using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using Common;
using DAL;
using IBLL;

namespace BLL
{
    public class MerchantBLL : IMerchantBLL, IDisposable
    {
        /// <summary>
        /// 私有的数据访问上下文
        /// </summary>
        protected SysEntities db;

        /// <summary>
        /// 构造函数，默认加载数据访问上下文
        /// </summary>
        public MerchantBLL()
        {
            db = new SysEntities();
        }

        /// <summary>
        /// 验证商家是否存在
        /// </summary>
        /// <param name="name">商家</param>
        /// <returns></returns>
        public bool CheckName(Merchant entity)
        {
            return db.Merchant.Any(o => o.Name == entity.Name && o.Id != entity.Id);
        }

        /// <summary>
        /// 检测用户是否已经使用
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool CheckSysPerson(Merchant entity)
        {
            using (var db=new SysEntities())
            {
                string sql = @"SELECT COUNT(SP.Id)AS Total FROM dbo.SysPerson SP
                    INNER JOIN dbo.MerchantSysPerson MSP ON SP.Id = MSP.SysPersonId
                    WHERE SP.Id=@personId AND MSP.MerchantId!=@MerchantId";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.AddWithValue("@personId",entity.SysPersonId);
                cmd.Parameters.AddWithValue("@MerchantId", entity.Id);
                var data = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db,cmd).Tables[0];
                foreach (DataRow item in data.Rows)
                {
                    if (Convert.ToInt32(item["Total"])>0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 查询的数据
        /// </summary>
        /// <param name="id">额外的参数</param>
        /// <param name="page">页码</param>
        /// <param name="rows">每页显示的行数</param>
        /// <param name="order">排序字段</param>
        /// <param name="sort">升序asc（默认）还是降序desc</param>
        /// <param name="search">查询条件</param>
        /// <param name="total">结果集的总数</param>
        /// <returns>结果集</returns>
        public List<Merchant> GetByParam(string merchantId, string id, int page, int rows, string order,
            string sort, string search, ref int total)
        {

            using (var repository = new MerchantRepository())
            {
                IQueryable<Merchant> queryData = null;
                queryData = repository.DaoChuData(db, order, sort, search);
                if (!string.IsNullOrEmpty(merchantId))
                {
                    queryData = queryData.Where(o=>o.ParentId==merchantId);
                }
                total = queryData.Count();
                if (total > 0)
                {
                    if (page <= 1)
                    {
                        queryData = queryData.Take(rows);
                    }
                    else
                    {
                        queryData = queryData.Skip((page - 1) * rows).Take(rows);
                    }
                    foreach (var item in queryData)
                    {
                        if (item.SysPerson != null)
                        {
                            item.SysPersonId = string.Empty;
                            foreach (var it in item.SysPerson)
                            {
                                item.SysPersonId += it.Name + ' ';
                            }
                        }
                    }
                }
                return queryData.ToList();
            }
        }

        /// <summary>
        /// 创建一个商家
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="db">数据库上下文</param>
        /// <param name="entity">一个商家</param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, SysEntities db, Merchant entity)
        {
            using (var repository = new MerchantRepository())
            {
                try
                {
                    int registerCodeId = 0;
                    if (CheckName(entity))
                    {
                        validationErrors.Add("商家名称已存在，请换一个新的商家名称");
                        return false;
                    }
                    if (!string.IsNullOrEmpty(entity.SysPersonId))
                    {
                        if (CheckSysPerson(entity))
                        {
                            validationErrors.Add("： 所分配的用户名已被其他商家使用，请换一个用户名");
                            return false;
                        }
                    }
                    if (!string.IsNullOrEmpty(entity.RegisterCode))
                    {
                        bool result = CheckRegiterCode(ref validationErrors, entity, ref registerCodeId);
                        if (result)
                        {

                            if (!string.IsNullOrEmpty(entity.SysPersonId))
                            {
                                SysPerson sys = new SysPerson { Id = entity.SysPersonId };
                                db.SysPerson.Attach(sys);
                                entity.SysPerson.Add(sys);
                            }

                            repository.Create(db, entity);
                            if (repository.Save(db) > 0)
                            {
                                string sql =
                                        @"UPDATE RegisterCode SET IsDistribution=1,MerchantId=@MerchantId WHERE Id=@Id";
                                var args = new DbParameter[] 
                                { 
                                     new SqlParameter { ParameterName = "Id", Value =registerCodeId},
                                     new SqlParameter{ParameterName="MerchantId",Value=entity.Id}
                                };
                                db.ExecuteStoreCommand(sql, args);
                                return true;
                            }
                        }
                        return false;
                    }
                    if (!string.IsNullOrEmpty(entity.SysPersonId))
                    {
                        SysPerson sys = new SysPerson { Id = entity.SysPersonId };
                        db.SysPerson.Attach(sys);
                        entity.SysPerson.Add(sys);
                    }

                    repository.Create(db, entity);
                    if (repository.Save(db) > 0)
                    {
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    validationErrors.Add(ex.Message);
                    ExceptionsHander.WriteExceptions(ex);
                }
                return false;
            }
        }

        /// <summary>
        /// 验证注册码
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool CheckRegiterCode(ref ValidationErrors validationErrors, Merchant entity, ref int registerCodeId)
        {

            var registerCode = db.RegisterCode.SingleOrDefault(o => o.Value == entity.RegisterCode);

            if (registerCode != null)
            {
                if (registerCode.MerchantId!=entity.Id)
                {
                    if (!registerCode.IsValid)
                    {
                        validationErrors.Add("：  输入的注册码已被注销，请与管理员联系！");
                        return false;
                    }
                    if (registerCode.IsUsed)
                    {
                        validationErrors.Add("：  输入的注册码已被使用！");
                        return false;
                    }
                    if (registerCode.IsDistribution &&registerCode.BelongMerchantId!=entity.Id)
                    {
                        validationErrors.Add("：  输入的注册码已被分配！");
                        return false;
                    }
                }
              
                registerCodeId = registerCode.Id;
                return true;
            }
            else
            {
                validationErrors.Add("：  输入的注册码不正确，请确认后再输入！");
                return false;
            }
           
        }

        /// <summary>
        /// 创建一个商家
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个商家</param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, Merchant entity)
        {
            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    var createSuccess = Create(ref validationErrors, db, entity);

                    if (createSuccess)
                    {
                        transactionScope.Complete();
                        return true;
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                validationErrors.Add(ex.Message);
                ExceptionsHander.WriteExceptions(ex);
            }
            return false;
        }

        /// <summary>
        /// 删除商家
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(ref ValidationErrors validationErrors, string id)
        {
            using (var repository = new MerchantRepository())
            {
                try
                {
                    return repository.Delete(id) == 1;
                }
                catch (Exception ex)
                {
                    validationErrors.Add(ex.Message);
                    ExceptionsHander.WriteExceptions(ex);
                }

                return false;
            }
        }

        /// <summary>
        /// 删除商家集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="deleteCollection">主键的商家</param>
        /// <returns></returns>    
        public bool DeleteCollection(ref ValidationErrors validationErrors, string[] deleteCollection)
        {
            using (var repository = new MerchantRepository())
            {
                try
                {
                    if (deleteCollection != null)
                    {
                        IQueryable<Merchant> collection = from f in db.Merchant
                                                        where deleteCollection.Contains(f.Id)
                                                        select f;
                        foreach (var item in collection)
                        {
                            var products = GetServiceProduct(item.Id);
                            if (products.Count>0)
                            {
                                validationErrors.Add("该商家下设置有服务产品，请先把其产品删除后再执行删除操作！");
                                return false;
                            }
                            var registerCodes = GetMerchantRegisterCodes(item.Id);
                            if (registerCodes.Count > 0)
                            {
                                validationErrors.Add("已经为该商家分配了注册码，请先把分配给它的注册码删除后再执行删除操作！");
                                return false;
                            }
                        }
                        using (TransactionScope transactionScope = new TransactionScope())
                        {
                            repository.Delete(db, deleteCollection);
                            if (deleteCollection.Length == repository.Save(db))
                            {
                                transactionScope.Complete();
                                return true;
                            }
                            else
                            {
                                Transaction.Current.Rollback();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    validationErrors.Add(ex.Message);
                    ExceptionsHander.WriteExceptions(ex);
                }
                return false;
            }
        }

        /// <summary>
        /// 获取商家服务产品
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        private List<ServiceProduct> GetServiceProduct(string merchantId)
        {
            using (var db=new SysEntities())
            {
                return db.ServiceProduct.Where(o => o.MerchantId == merchantId).ToList();
            }
        }

        /// <summary>
        /// 获取商家注册码
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        private List<RegisterCode> GetMerchantRegisterCodes(string merchantId)
        {
            using (var db = new SysEntities())
            {
                return db.RegisterCode.Where(o => o.BelongMerchantId == merchantId).ToList();
            }
        }

        /// <summary>
        ///  创建商家集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entitys">商家集合</param>
        /// <returns></returns>
        public bool EditCollection(ref ValidationErrors validationErrors, IQueryable<Merchant> entitys,int editAction)
        {
            if (entitys != null)
            {
                try
                {
                    int flag = 0, count = entitys.Count();
                    if (count > 0)
                    {
                        using (TransactionScope transactionScope = new TransactionScope())
                        {
                            foreach (var entity in entitys)
                            {
                                if (Edit(ref validationErrors, db, entity, editAction))
                                {
                                    flag++;
                                }
                                else
                                {
                                    Transaction.Current.Rollback();
                                    return false;
                                }
                            }
                            if (count == flag)
                            {
                                transactionScope.Complete();
                                return true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    validationErrors.Add(ex.Message);
                    ExceptionsHander.WriteExceptions(ex);
                }
            }
            return false;
        }

        /// <summary>
        /// 编辑一个商家
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="db">数据上下文</param>
        /// <param name="entity">一个商家</param>
        /// <returns>是否编辑成功</returns>
        public bool Edit(ref ValidationErrors validationErrors, SysEntities db, Merchant entity,int editAction)
        {
            using (var repository = new MerchantRepository())
            {
                if (entity == null)
                {
                    return false;
                }
                if (CheckName(entity))
                {
                    validationErrors.Add("商家名称已被占用，请换一个新的商家名称");
                    return false;
                }

                int count = 1;
                var merchant = GetById(entity.Id);
                if (entity.Logo == null)
                {
                    entity.Logo = merchant.Logo;
                }
                int registerCodeId = 0;
                if (!string.IsNullOrEmpty(entity.OldRegisterCode))
                {
                    entity.RegisterCode = entity.OldRegisterCode;
                }
                if (!EditMerchantSysPersonRelation(ref validationErrors,entity))
                {
                    return false;
                }
              

                if (entity.RegisterCode != entity.OldRegisterCode &&editAction==0)
                {
                    bool result = CheckRegiterCode(ref validationErrors, entity, ref registerCodeId);
                    if (result)
                    {
                        string sql = @"UPDATE RegisterCode SET IsDistribution=1,MerchantId=@MerchantId WHERE Id=@Id";
                        var args = new DbParameter[] 
                                { 
                                     new SqlParameter { ParameterName = "Id", Value =registerCodeId},
                                     new SqlParameter{ParameterName="MerchantId",Value=entity.Id}
                                };
                        db.ExecuteStoreCommand(sql, args);
                        Merchant edit = repository.Edit(db, entity);
                       
                        if (count == repository.Save(db))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                Merchant editEntity = repository.Edit(db, entity);
                if (repository.Save(db)>0)
                {
                    var syspeopleId = GetSysPersonId(entity.Id);
                    var members = GetMembers(syspeopleId);
                    if (!string.IsNullOrEmpty(syspeopleId)&&members.Count>0)
                    {
                        bool isVisable = entity.IsVisible == 1 ? true : false;
                        UpdateMemberIsVisable(syspeopleId, isVisable);
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    validationErrors.Add("编辑商家出错了");
                }
                return false;
            }
        }

        /// <summary>
        /// 编辑商家用户
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool EditMerchantSysPersonRelation(ref ValidationErrors validationErrors, Merchant entity)
        {
            using (var db=new SysEntities())
            {
                string deleteSql = @"DELETE FROM MerchantSysPerson WhERE MerchantId=@MerchantId";
                SqlCommand scmd1 = new SqlCommand(deleteSql);
                scmd1.Parameters.AddWithValue("@MerchantId", entity.Id);
                ObjectQueryExtensions.ExecuteNonQuery(db, scmd1);
                if (!string.IsNullOrEmpty(entity.SysPersonId))
                {
                    if (CheckSysPerson(entity))
                    {
                        validationErrors.Add("： 所分配的用户名已被其他商家使用，请换一个用户名");
                        return false;
                    }
                    string insertSql = @"INSERT INTO MerchantSysPerson(SysPersonId,MerchantId)VALUES(@SysPersonId,@MerchantId)";
                    SqlCommand scmd2 = new SqlCommand(insertSql);
                    scmd2.Parameters.AddWithValue("@MerchantId", entity.Id);
                    scmd2.Parameters.AddWithValue("@SysPersonId", entity.SysPersonId);
                    ObjectQueryExtensions.ExecuteNonQuery(db, scmd2);
                    return true;
                }
                return true;
            }
        }

        /// <summary>
        /// 编辑一个商家
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个商家</param>
        /// <returns>是否编辑成功</returns>
        public bool Edit(ref ValidationErrors validationErrors, Merchant entity,int editAction)
        {
            try
            {
                SysEntities newDb = new SysEntities();
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (Edit(ref validationErrors, newDb, entity,editAction))
                    {
                        transactionScope.Complete();
                        return true;
                    }
                    else
                    {
                        Transaction.Current.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                validationErrors.Add(ex.Message);
                ExceptionsHander.WriteExceptions(ex);
            }
            return false;
        }

        /// <summary>
        /// 获取所有商家
        /// </summary>
        /// <returns></returns>
        public List<Merchant> GetAll()
        {
            using (var repository = new MerchantRepository())
            {
                return repository.GetAll(db).ToList();
            }
        }

        /// <summary>
        /// 获取商家用户Id
        /// </summary>
        /// <param name="merchantId">商家Id</param>
        /// <returns></returns>
        private string GetSysPersonId(string merchantId)
        {
            using (SysEntities db = new SysEntities())
            {
                string sysPersonId = string.Empty;
                string sql = @"
                    SELECT SysPersonId,MerchantId FROM MerchantSysPerson
                    WHERE MerchantId=@MerchantId";
                SqlCommand scmd = new SqlCommand(sql);
                scmd.Parameters.AddWithValue("@MerchantId", merchantId);
                var dataSource = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd).Tables[0];
                foreach (DataRow item in dataSource.Rows)
                {
                    sysPersonId = item["SysPersonId"].ToString() == null ? "" : item["SysPersonId"].ToString();
                }

                return sysPersonId;
            }
        }


        /// <summary>
        /// 获取商家用户Id
        /// </summary>
        /// <param name="merchantId">商家Id</param>
        /// <returns></returns>
        public string GetMerchantId(string syspersonId)
        {
            using (SysEntities db = new SysEntities())
            {
                string merchantId = string.Empty;
                string sql = @"
                    SELECT SysPersonId,MerchantId FROM MerchantSysPerson
                    WHERE SysPersonId=@SysPersonId";
                SqlCommand scmd = new SqlCommand(sql);
                scmd.Parameters.AddWithValue("@SysPersonId", syspersonId);
                var dataSource = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd).Tables[0];
                foreach (DataRow item in dataSource.Rows)
                {
                    merchantId = item["MerchantId"].ToString() == null ? "" : item["MerchantId"].ToString();
                }

                return merchantId;
            }
        }

        /// <summary>
        /// 获取商家会员
        /// </summary>
        /// <param name="sysPeopleId"></param>
        /// <returns></returns>
        private List<Member> GetMembers(string sysPeopleId)
        {
            using (var db = new SysEntities())
            {
                return db.Member.Where(o => o.CreatePersonId==sysPeopleId).ToList();
            }
        }

        /// <summary>
        /// 更新会员可见性（正对下级上级对上级商家有用）
        /// </summary>
        /// <param name="sysPeopleId"></param>
        /// <param name="isVisable"></param>
        private void UpdateMemberIsVisable(string sysPeopleId, bool isVisable)
        {
            using (var db=new SysEntities())
            {
                string sql = @"UPDATE Member SET IsVisible=@IsVisible WHERE CreatePersonId=@CreatePersonId";
                var args = new DbParameter[]
                    {
                        new SqlParameter{ParameterName="IsVisible",Value=isVisable},
                        new SqlParameter{ParameterName="CreatePersonId",Value=sysPeopleId}
                    };
                db.ExecuteStoreCommand(sql, args);
            }
        }

        /// <summary>
        /// 获取一个商家类别下的商家
        /// </summary>
        /// <param name="merchantTypeId">商家类别Id</param>
        /// <returns></returns>
        public List<Merchant> GetMerchantsByMerchantTypeId(int merchantTypeId)
        {
            using (var db=new SysEntities())
            {
                return db.Merchant.Where(o => o.MerchantTypeId == merchantTypeId).ToList();
            }
        }

        /// <summary>
        /// 获取所有商家
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetAllMerchant()
        {
            using (var dataContext = new SysEntities())
            {
                var data = (from m in dataContext.Merchant
                            join c in dataContext.RegisterCode
                            on m.Id equals c.MerchantId
                            select new
                            {
                                Id = m.Id,
                                Name = m.Name,
                                ParentId = m.ParentId,
                                Code = c.Value
                            }).ToList();
                return (dynamic)data;
            }
        }

        /// <summary>
        /// 根据主键获取一个商家
        /// </summary>
        /// <param name="id">商家的主键</param>
        /// <returns>一个商家</returns>
        public Merchant GetById(string id)
        {
            using (var repository = new MerchantRepository())
            {
                return repository.GetById(db, id);
            }
        }

        /// <summary>
        /// 获取在该表一条数据中，出现的所有外键实体
        /// </summary>
        /// <param name="id">外键</param>
        /// <returns>外键实体集合</returns>
        public Merchant GetRefSysPerson(string id)
        {
            using (var repository = new MerchantRepository())
            {
                return repository.GetRefSysPerson(db, id).FirstOrDefault();
            }
        }

        /// <summary>
        /// 设置商家关系
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="oldParentId"></param>
        /// <param name="newParentId"></param>
        /// <param name="oldChildIds"></param>
        /// <param name="newChildIds"></param>
        /// <param name="validationErrors"></param>
        /// <returns></returns>
        public bool SetRelationShip(string merchantId, string oldParentId, string newParentId, 
            string[] oldChildIds, string[] newChildIds, ref ValidationErrors validationErrors)
        {
            if (string.IsNullOrWhiteSpace(merchantId))
            {
                validationErrors.Add("商家Id不能为空！");
                return false;
            }

            using (var dataContext = new SysEntities())
            {
                var merchant = dataContext.Merchant
                                .Where(m => m.Id == merchantId).FirstOrDefault();
                if (merchant == null)
                {
                    validationErrors.Add("商家Id无效！");
                    return false;
                }

                merchant.ParentId = newParentId;

                var newParent = dataContext.Merchant
                                            .Where(m => m.Id == newParentId)
                                            .FirstOrDefault();
                if (newParent != null)
                {
                    if (newParent.ParentId == merchant.Id)
                    {
                        newParent.ParentId = null;
                    }
                }

                if (oldChildIds != null && oldChildIds.Length > 0)
                {
                    var oldChildren = dataContext.Merchant
                                            .Where(m => oldChildIds.Contains(m.Id))
                                            .ToList();
                    oldChildren.ForEach(m =>
                    {
                        m.ParentId = null;
                    });
                }
                if (newChildIds != null && newChildIds.Length > 0)
                {
                    var newChildren = dataContext.Merchant
                                            .Where(m => newChildIds.Contains(m.Id))
                                            .ToList();
                    newChildren.ForEach(m =>
                    {
                        m.ParentId = merchantId;
                    });
                }
                dataContext.SaveChanges();
            }
            return true;
        }

        /// <summary>
        /// 修改商家资料
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ChangeSelfInfo(ref ValidationErrors validationErrors, Merchant entity)
        {
            try
            {
                string sql = @"UPDATE Merchant SET Contacts=@Contacts,Cellphone=@Cellphone,
                            SiteUrl=@SiteUrl,Address=@Address,Telephone=@Telephone,Description=@Description,Logo=@Logo
                            WHERE Id=@Id";
                

                var args = new DbParameter[] 
                {
                    //new SqlParameter{ParameterName="Contacts",Value=(object)entity.Contacts??DBNull.Value},
                    // new SqlParameter{ParameterName="Cellphone",Value=(object)entity.Cellphone??DBNull.Value},
                    //  new SqlParameter{ParameterName="SiteUrl",Value=(object)entity.SiteUrl??DBNull.Value},
                    //   new SqlParameter{ParameterName="Address",Value=(object)entity.Address??DBNull.Value},
                    //    new SqlParameter{ParameterName="Telephone",Value=(object)entity.Telephone??DBNull.Value},
                    //     new SqlParameter{ParameterName="Description",Value=(object)entity.Description??DBNull.Value},
                    //      new SqlParameter{ParameterName="Logo",Value=entity.Logo==null?null:entity.Logo},
                    //       new SqlParameter{ParameterName="Id",Value=entity.Id},
                    //       new SqlParameter("")    
                      
                };

                SqlParameter[] sqlParams = new SqlParameter[]{
                new SqlParameter("@Contacts", SqlDbType.NVarChar,50),
                new SqlParameter("@Cellphone", SqlDbType.NVarChar, 50),
                new SqlParameter("@SiteUrl", SqlDbType.NVarChar, 200),
                new SqlParameter("@Address", SqlDbType.NVarChar, 200),
                new SqlParameter("@Telephone", SqlDbType.NVarChar, 50),
                new SqlParameter("@Description", SqlDbType.NVarChar, 200),
                new SqlParameter("@Logo", SqlDbType.VarBinary),
                new SqlParameter("@Id", SqlDbType.NVarChar, 36),
                 };

                sqlParams[0].Value = entity.Contacts;
                sqlParams[1].Value = entity.Cellphone;
                sqlParams[2].Value =(object) entity.SiteUrl ?? DBNull.Value;
                sqlParams[3].Value = (object)entity.Address??DBNull.Value;
                sqlParams[4].Value = (object)entity.Telephone ?? DBNull.Value;
                sqlParams[5].Value = (object)entity.Description ?? DBNull.Value;
                sqlParams[6].Value = (object)entity.Logo ?? DBNull.Value;
                sqlParams[7].Value = entity.Id;
                db.ExecuteStoreCommand(sql, sqlParams);
                return true;
            }
            catch (Exception ex)
            {
                validationErrors.Add(ex.Message);
                ExceptionsHander.WriteExceptions(ex);
            }
            return false;
        }

        public void Dispose()
        {

        }
    }
}
