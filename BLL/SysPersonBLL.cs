using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using DAL;
using Common;
using System.Data.SqlClient;
using System.Data;

namespace BLL
{
    /// <summary>
    /// 人员 
    /// </summary>
    public class SysPersonBLL : IBLL.ISysPersonBLL, IDisposable
    {
        /// <summary>
        /// 私有的数据访问上下文
        /// </summary>
        protected SysEntities db;
        /// <summary>
        /// 人员的数据库访问对象
        /// </summary>
        SysPersonRepository repository = new SysPersonRepository();
        /// <summary>
        /// 构造函数，默认加载数据访问上下文
        /// </summary>
        public SysPersonBLL()
        {
            db = new SysEntities();
        }
        /// <summary>
        /// 已有数据访问上下文的方法中调用
        /// </summary>
        /// <param name="entities">数据访问上下文</param>
        public SysPersonBLL(SysEntities entities)
        {
            db = entities;
        }
        /// <summary>
        /// 验证用户名是否存在,true标示已经存在
        /// </summary>
        /// <param name="name">用户名</param>
        /// <returns></returns>
        public bool CheckName(string name)
        {
            return db.SysPerson.Any(a => a.Name == name);
        }
        /// <summary>
        /// 验证用户名是否改变
        /// </summary>
        /// <param name="name">用户名</param>
        /// <returns></returns>
        public bool CheckName(SysPerson sysPerson)
        {
            using (SysEntities dbCheckName = new SysEntities())
            {
                SysPerson person = dbCheckName.SysPerson.FirstOrDefault(f => f.Id == sysPerson.Id);
                if (person != null)
                {
                    if (sysPerson.Name == person.Name)
                    {
                        return false;//没有修改用户名
                    }
                    else
                    {
                        return CheckName(sysPerson.Name);
                    }
                }
            }
            return true;
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
        public List<SysPerson> GetByParam(string id, int page, int rows, string order, string sort, string search, ref int total)
        {


            IQueryable<SysPerson> queryData = repository.DaoChuData(db, order, sort, search);
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
                    if (item.SysRole != null)
                    {
                        item.SysRoleId = string.Empty;
                        foreach (var it in item.SysRole)
                        {
                            item.SysRoleId += it.Name + ' ';
                        }
                    }
                    if (item.Merchant!=null)
                    {
                        item.MerchantId = string.Empty;
                        foreach (var it in item.Merchant)
                        {
                            item.MerchantId+=it.Name+' ';
                        }
                    }

                }

            }
            return queryData.ToList();
        }

        /// <summary>
        /// 创建一个人员
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="db">数据库上下文</param>
        /// <param name="entity">一个人员</param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, SysEntities db, SysPerson entity)
        {
            int count = 1;
            if (CheckName(entity.Name))
            {
                validationErrors.Add("用户名已被占用，请换一个新的用户名");
                return false;
            }
            foreach (string item in entity.SysRoleId.GetIdSort())
            {
                SysRole sys = new SysRole { Id = item };
                db.SysRole.Attach(sys);
                entity.SysRole.Add(sys);
                count++;
            }

            repository.Create(db, entity);
            if (count == repository.Save(db))
            {
                return true;
            }
            else
            {
                validationErrors.Add("创建出错了");
            }
            return false;
        }
        /// <summary>
        /// 创建一个人员
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个人员</param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, SysPerson entity)
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
        ///  创建人员集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entitys">人员集合</param>
        /// <returns></returns>
        public bool CreateCollection(ref ValidationErrors validationErrors, IQueryable<SysPerson> entitys)
        {
            try
            {
                if (entitys != null)
                {
                    int flag = 0, count = entitys.Count();
                    if (count > 0)
                    {
                        using (TransactionScope transactionScope = new TransactionScope())
                        {
                            foreach (var entity in entitys)
                            {
                                if (Create(ref validationErrors, db, entity))
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
            }
            catch (Exception ex)
            {
                validationErrors.Add(ex.Message);
                ExceptionsHander.WriteExceptions(ex);
            }
            return false;
        }

        /// <summary>
        /// 删除一个人员
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="id">一个人员的主键</param>
        /// <returns></returns>  
        public bool Delete(ref ValidationErrors validationErrors, string id)
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
        /// <summary>
        /// 删除人员集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="deleteCollection">主键的人员</param>
        /// <returns></returns>    
        public bool DeleteCollection(ref ValidationErrors validationErrors, string[] deleteCollection)
        {
            try
            {
                if (deleteCollection != null)
                {
                    var merchantSysPersonCount = GetMerchantSysPerson(deleteCollection[0]);
                    if (merchantSysPersonCount>0)
                    {
                          validationErrors.Add("该用户正在商家使用，不可删除，请先修改正在使用的商家用户！");
                          return false;
                    }
                    var infoPersonCount = GetInfoSysPerson(deleteCollection[0]);
                    if (infoPersonCount > 0)
                    {
                        validationErrors.Add("该用户已经发布了信息，不可删除，若要删除请先删除该用户发布的信息！");
                        return false;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetMerchantSysPerson(string id)
        {
            using (var dataContext = new SysEntities())
            {
                int count = 0;
                string sql = @"
                    SELECT  COUNT(SysPersonId) AS Count
                    FROM    dbo.MerchantSysPerson
                    WHERE   SysPersonId = @SysPersonId";
                SqlCommand scmd = new SqlCommand(sql);
                scmd.Parameters.AddWithValue("@SysPersonId", id);
                var data = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(dataContext, scmd).Tables[0];
                foreach (DataRow item in data.Rows)
                {
                    count = Convert.ToInt32(item["Count"]);
                }

                return count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetInfoSysPerson(string id)
        {
            using (var dataContext = new SysEntities())
            {
                int count = 0;
                string sql = @"
                    SELECT  COUNT(CreatePersonId) AS Count
                    FROM    dbo.Informations
                    WHERE   CreatePersonId = @CreatePersonId";
                SqlCommand scmd = new SqlCommand(sql);
                scmd.Parameters.AddWithValue("@CreatePersonId", id);
                var data = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(dataContext, scmd).Tables[0];
                foreach (DataRow item in data.Rows)
                {
                    count = Convert.ToInt32(item["Count"]);
                }

                return count;
            }
        }

        /// <summary>
        ///  创建人员集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entitys">人员集合</param>
        /// <returns></returns>
        public bool EditCollection(ref ValidationErrors validationErrors, IQueryable<SysPerson> entitys)
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
                                if (Edit(ref validationErrors, db, entity))
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
        /// 编辑一个人员
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="db">数据上下文</param>
        /// <param name="entity">一个人员</param>
        /// <returns>是否编辑成功</returns>
        public bool Edit(ref ValidationErrors validationErrors, SysEntities db, SysPerson entity)
        {  /*                       
                           * 不操作 原有 现有
                           * 增加   原没 现有
                           * 删除   原有 现没
                           */
            if (entity == null)
            {
                return false;
            }
            if (CheckName(entity))
            {
                validationErrors.Add("用户名已被占用，请换一个新的用户名");
                return false;
            }

            int count = 1;
            SysPerson editEntity = repository.Edit(db, entity);

            List<string> addSysRoleId = new List<string>();
            List<string> deleteSysRoleId = new List<string>();
            DataOfDiffrent.GetDiffrent(entity.SysRoleId.GetIdSort(), entity.SysRoleIdOld.GetIdSort(), ref addSysRoleId, ref deleteSysRoleId);
            if (addSysRoleId != null && addSysRoleId.Count() > 0)
            {
                foreach (var item in addSysRoleId)
                {
                    SysRole sys = new SysRole { Id = item };
                    db.SysRole.Attach(sys);
                    editEntity.SysRole.Add(sys);
                    count++;
                }
            }
            if (deleteSysRoleId != null && deleteSysRoleId.Count() > 0)
            {
                List<SysRole> listEntity = new List<SysRole>();
                foreach (var item in deleteSysRoleId)
                {
                    SysRole sys = new SysRole { Id = item };
                    listEntity.Add(sys);
                    db.SysRole.Attach(sys);
                }
                foreach (SysRole item in listEntity)
                {
                    editEntity.SysRole.Remove(item);//查询数据库
                    count++;
                }
            }

            if (count == repository.Save(db))
            {
                return true;
            }
            else
            {
                validationErrors.Add("编辑人员出错了");
            }
            return false;
        }
        /// <summary>
        /// 编辑一个人员
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个人员</param>
        /// <returns>是否编辑成功</returns>
        public bool Edit(ref ValidationErrors validationErrors, SysPerson entity)
        {
            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (Edit(ref validationErrors, db, entity))
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
        public List<SysPerson> GetAll()
        {
            return repository.GetAll(db).ToList();
        }

        /// <summary>
        /// 根据主键获取一个人员
        /// </summary>
        /// <param name="id">人员的主键</param>
        /// <returns>一个人员</returns>
        public SysPerson GetById(string id)
        {
            return repository.GetById(db, id);
        }

        /// <summary>
        /// 获取在该表一条数据中，出现的所有外键实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>外键实体集合</returns>
        public List<SysRole> GetRefSysRole(string id)
        {
            return repository.GetRefSysRole(db, id).ToList();
        }
        /// <summary>
        /// 获取在该表中出现的所有外键实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>外键实体集合</returns>
        public List<SysRole> GetRefSysRole()
        {
            return repository.GetRefSysRole(db).ToList();
        }

        /// <summary>
        /// 获取在该表一条数据中，出现的所有外键实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>外键实体集合</returns>
        public List<Merchant> GetRefMerchant(string id)
        {
            return repository.GetRefMerchant(db,id).ToList();
        }

        public void Dispose()
        {

        }
    }
}

