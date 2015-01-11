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
    public class MemberBLL : IMemberBLL, IDisposable
    {
        /// <summary>
        /// 私有的数据访问上下文
        /// </summary>
        protected SysEntities db;

        public MemberBLL()
            : this(new SysPersonBLL())
        {
        }

        ISysPersonBLL _iSysPersonBll;
        /// <summary>
        /// 构造函数，默认加载数据访问上下文
        /// </summary>
        public MemberBLL(SysPersonBLL sysPersonBll)
        {
            _iSysPersonBll = sysPersonBll;
            db = new SysEntities();
        }


        /// <summary>
        /// 验证会员名称是否存在
        /// </summary>
        /// <param name="entity">会员实体</param>
        /// <returns></returns>
        public bool CheckName(Member entity)
        {
            return db.Member.Any(o => o.Name == entity.Name && o.CreatePersonId == entity.CreatePersonId &&
                o.Id != entity.Id);
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
        public List<Member> GetByParam(string sysPersonId,string id, int page, int rows, string order,
              string sort, string search, ref int total)
        {

            using (var repository = new MemberRepository())
            {
                IQueryable<Member> queryData = null;
                var sysRole = _iSysPersonBll.GetRefSysRole(sysPersonId).FirstOrDefault();
                if (sysRole.Power == 1 || sysRole.Power == 2)
                {
                   
                   queryData = repository.DaoChuData(db, order, sort, search,null);
                }
                else
                {
                    queryData = repository.DaoChuData(db, order, sort, search, sysPersonId)
                        .Where(o =>  o.IsValid == true);
                    
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
                        if (item.MemberGroup != null)
                        {
                            item.MemberGroupId = string.Empty;
                            foreach (var it in item.MemberGroup)
                            {
                                item.MemberGroupId += it.Name + ' ';
                            }
                        }
                    }
                }
                return queryData.ToList();
            }
        }

        /// <summary>
        /// 验证注册码
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool CheckRegiterCode(ref ValidationErrors validationErrors, Member entity,ref int registerCodeId)
        {

            var registerCode = db.RegisterCode.SingleOrDefault(o => o.Value == entity.RegisterCode);
            if (registerCode != null)
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
                var sysPersonId = GetSysPersonId(entity.CreatePersonId);
                var result = !string.IsNullOrEmpty(registerCode.BelongMerchantId) && sysPersonId==entity.CreatePersonId;
                if (registerCode.IsDistribution && !result)
                {
                    validationErrors.Add("：  输入的注册码已被分配其他商家！");
                    return false;
                }

                if (!string.IsNullOrEmpty(registerCode.MemberId)
                    || !string.IsNullOrEmpty(registerCode.MerchantId))
                {
                    validationErrors.Add("：  输入的注册码已被分配！");
                    return false;
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
        /// 根据登录用户获取对应的信息（正对于商家用户）
        /// </summary>
        /// <param name="syspersonId"></param>
        /// <returns></returns>
        private string GetSysPersonId(string syspersonId)
        {
            using (SysEntities db = new SysEntities())
            {
                string sysPersonId = string.Empty;
                string sql = @"
                    SELECT SysPersonId,MerchantId FROM MerchantSysPerson
                    WHERE SysPersonId=@SysPersonId";
                SqlCommand scmd = new SqlCommand(sql);
                scmd.Parameters.AddWithValue("@SysPersonId", syspersonId);
                var dataSource = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db,scmd).Tables[0];
                foreach (DataRow item in dataSource.Rows)
                {
                    sysPersonId = item["SysPersonId"].ToString() == null ? "" : item["SysPersonId"].ToString();
                }

                return sysPersonId;
            }
        }

        /// <summary>
        /// 添加会员
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors,SysEntities db,  Member entity)
        {
            using (var repository = new MemberRepository())
            {
                try
                {
                    int registerCodeId = 0;
                    if (CheckName(entity))
                    {
                        validationErrors.Add("：  已经有一个相同的会员名称，请换一个新的会员名称");
                        return false;
                    }
                    if (!string.IsNullOrEmpty(entity.RegisterCode))
                    {
                        bool result=CheckRegiterCode(ref validationErrors,entity,ref registerCodeId);
                        if (result)
                        {
                            repository.Create(entity);
                            string sql = @"UPDATE RegisterCode SET IsDistribution=1,MemberId=@MemberId WHERE Id=@Id";
                            var args = new DbParameter[] 
                                { 
                                     new SqlParameter { ParameterName = "Id", Value =registerCodeId},
                                     new SqlParameter{ParameterName="MemberId",Value=entity.Id}
                                };
                            db.ExecuteStoreCommand(sql, args);
                            return true;
                        }
                        return false;
                    }
                    repository.Create(entity);
                    return true;
                   
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
        /// 添加会员
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, Member entity)
        {
            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (Create(ref validationErrors, db, entity))
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
        /// 删除会员
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(ref ValidationErrors validationErrors, string id)
        {
            using (var repository = new MemberRepository())
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
        /// 删除会员集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="deleteCollection">主键的会员</param>
        /// <returns></returns>    
        public bool DeleteCollection(string sysPersonId, ref ValidationErrors validationErrors, string[] deleteCollection)
        {
            using (var repository = new MemberRepository())
            {
                try
                {
                    if (deleteCollection != null)
                    {
                        IQueryable<Member> collection = from f in db.Member
                                                        where deleteCollection.Contains(f.Id)
                                                        select f;
                        foreach (var item in collection)
                        {
                            Member entity = GetById(item.Id);
                            var sysRole = _iSysPersonBll.GetRefSysRole(sysPersonId).FirstOrDefault();
                            if (entity!=null&&entity.CreatePersonId!=sysPersonId&&sysRole.Power==3)
                            {
                                 validationErrors.Add("： 会员 【'"+item.Name+"'】为下级商家的会员，你没有权限删除！");
                                return false;
                            }
                            var memberInMemberGroup = GetMember_MemberGroupList(item.Id).FirstOrDefault();
                            if (memberInMemberGroup != null)
                            {
                                validationErrors.Add("： 会员 【'"+item.Name+"'】 已存在会员分组【 "+memberInMemberGroup.Name+"】 下，"+
                                "不可删除，请先从所在分组中删除该会员!");
                                return false;
                            }
                            if (!string.IsNullOrEmpty(item.RegisterCode))
                            {
                                UpdateRegisterCode(item.RegisterCode);
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
        /// 更新注册码
        /// </summary>
        /// <param name="code"></param>
        private void UpdateRegisterCode(string code)
        {
            using (var db = new SysEntities())
            {
                var entity = db.RegisterCode.SingleOrDefault(o => o.Value == code);
                entity.IsUsed = false;
                entity.IsValid = true;
                entity.MemberId = null;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 获取获取分组列表
        /// </summary>
        /// <param name="memberid"></param>
        /// <returns></returns>
        private IList<Member_MemberGroup> GetMember_MemberGroupList(string memberid)
        {
            List<Member_MemberGroup> list = new List<Member_MemberGroup>();
            using (var db=new SysEntities())
            {
                try
                {
                    string sql = @"
                            SELECT  MemberId ,
                                    MemberGroupId ,
                                    MG.Name
                            FROM    Member_MemberGroup MM
                                    INNER JOIN MemberGroup MG ON MM.MemberGroupId = MG.Id
                            WHERE   MemberId = @MemberId";
                    SqlCommand scmd = new SqlCommand(sql);
                    scmd.Parameters.AddWithValue("@MemberId",memberid);
                    var dataSources = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd).Tables[0];

                    foreach (DataRow item in dataSources.Rows)
                    {
                        var member_MemberGroup = new Member_MemberGroup();
                        member_MemberGroup.MemberId = item["MemberId"].ToString();
                        member_MemberGroup.MemberGroupId = item["MemberGroupId"].ToString();
                        member_MemberGroup.Name = item["Name"].ToString();
                        list.Add(member_MemberGroup);
                    }
                    return list;
                }
                catch (Exception)
                {
                    throw;
                }   
            }
        }

        /// <summary>
        /// 编辑一个会员
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个数据字典</param>
        /// <returns></returns>
        public bool Edit(ref ValidationErrors validationErrors,SysEntities db, Member entity)
        {
            using (var repository = new MemberRepository())
            {
                try
                {
                    int registerCodeId = 0;
                    if (CheckName(entity))
                    {
                        validationErrors.Add("：  已经有一个相同的会员名称，请换一个新的会员名称");
                        return false;
                    }
                    if (!string.IsNullOrEmpty(entity.OldRegisterCode))
                    {
                        entity.RegisterCode = entity.OldRegisterCode;
                    }
                    if (entity.RegisterCode != entity.OldRegisterCode)
                    {
                        bool result=CheckRegiterCode(ref validationErrors, entity, ref registerCodeId);
                        if (result)
                        {

                            string sql = @"UPDATE RegisterCode SET IsDistribution=1,MemberId=@MemberId WHERE Id=@Id";
                            var args = new DbParameter[] 
                                { 
                                     new SqlParameter { ParameterName = "Id", Value =registerCodeId},
                                     new SqlParameter{ParameterName="MemberId",Value=entity.Id}
                                };
                            db.ExecuteStoreCommand(sql, args);
                            repository.Edit(db, entity);
                            repository.Save(db);
                            return true;
                        }
                        return false;
                    }

                    repository.Edit(db, entity);
                    repository.Save(db);
                    return true;
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
        /// 编辑一个会员
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个会员</param>
        /// <returns>是否编辑成功</returns>
        public bool Edit(string sysPersonId, ref ValidationErrors validationErrors, Member entity)
        {
            using (var repository = new MemberGroupRepository())
            {
                try
                {
                    var sysRole = _iSysPersonBll.GetRefSysRole(sysPersonId).FirstOrDefault();
                    if (entity != null && entity.CreatePersonId != sysPersonId && sysRole.Power == 3)
                    {
                        validationErrors.Add("： 会员 【'" + entity.Name + "'】为下级商家的会员，你没有权限修改其信息！");
                        return false;
                    }
                    SysEntities newDb = new SysEntities();
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        if (Edit(ref validationErrors, newDb, entity))
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
        }

        /// <summary>
        /// 获取所有会员
        /// </summary>
        /// <returns></returns>
        public List<Member> GetAll()
        {
            using (var repository = new MemberRepository())
            {
                return repository.GetAll(db).ToList();
            }
        }

        /// <summary>
        /// 根据主键获取一个会员
        /// </summary>
        /// <param name="id">会员的主键</param>
        /// <returns>一个会员</returns>
        public Member GetById(string id)
        {
            using (var repository = new MemberRepository())
            {
                return repository.GetById(db, id);
            }
        }

        /// <summary>
        /// 设置（注销、恢复、设置VIP）
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个对象</param>
        /// <returns></returns>
        public bool Setup(ref ValidationErrors validationErrors, Member entity)
        {
            try
            {
                string sql = @"UPDATE Member SET IsValid=@IsValid,VIP=@VIP WHERE Id=@Id";
                var args = new DbParameter[]
                    {
                        new SqlParameter{ParameterName="IsValid",Value=entity.IsValid},
                        new SqlParameter{ParameterName="VIP",Value=entity.VIP},
                        new SqlParameter{ParameterName="Id",Value=entity.Id}
                    };
                db.ExecuteStoreCommand(sql, args);
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
