using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using Common;
using DAL;
using IBLL;

namespace BLL
{
    public class ServiceProductBLL : IServiceProductBLL, IDisposable
    {
        /// <summary>
        /// 私有的数据访问上下文
        /// </summary>
        protected SysEntities db;

         public ServiceProductBLL()
            : this(new SysPersonBLL())
        {
        }

        ISysPersonBLL _iSysPersonBll;
        /// <summary>
        /// 构造函数，默认加载数据访问上下文
        /// </summary>
        public ServiceProductBLL(SysPersonBLL sysPersonBll)
        {
            _iSysPersonBll = sysPersonBll;
            db = new SysEntities();
        }

        /// <summary>
        /// 验证服务产品名称是否存在
        /// </summary>
        /// <param name="entity">分组实体</param>
        /// <returns></returns>
        public bool CheckName(ServiceProduct entity)
        {
            return db.ServiceProduct.Any(o => o.Name == entity.Name &&o.MerchantId==entity.MerchantId&&
                o.Id!=entity.Id);
        }

        /// <summary>
        /// 获取商家服务产品
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public List<ServiceProduct> GetMerchantServiceProduct(string merchantId)
        {
            return db.ServiceProduct.Where(o => o.MerchantId == merchantId).ToList();
        }

        /// <summary>
        /// 根据商家Id获取商家信息
        /// </summary>
        /// <param name="id">商家id</param>
        /// <returns></returns>
        public Merchant GetMerchantById(string id)
        {
            return db.Merchant.FirstOrDefault(o => o.Id == id);
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
        public List<ServiceProduct> GetByParam(string sysPersonId, int id, int page, int rows, string order,
            string sort, string search, ref int total)
        {

            using (var  repository = new ServiceProductRepository())
            {
                IQueryable<ServiceProduct> queryData = null;
                var sysRole = _iSysPersonBll.GetRefSysRole(sysPersonId).FirstOrDefault();
                var merchant = _iSysPersonBll.GetRefMerchant(sysPersonId).FirstOrDefault();
                if (sysRole.Power == 1 || sysRole.Power == 2)
                {
                    queryData = repository.DaoChuData(db, order, sort, search);
                }
                else
                {
                    queryData = repository.DaoChuData(db, order, sort, search)
                        .Where(o => o.MerchantId == merchant.Id);
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

                }
                return queryData.ToList();
            }
        }

        /// <summary>
        /// 产品星级设置验证
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool StarValidation( ValidationErrors validationErrors,ServiceProduct entity,string action)
        {
            var ComprehensiveStar = GetMerchantById(entity.MerchantId).ComprehensiveStar;//商家综合星级
            if (ComprehensiveStar == 0)
            {
                validationErrors.Add(": 当前商家的综合星级为0，不能为其服务产品设置星级！");
                return false;
            }
            if (entity.Star > ComprehensiveStar)
            {
                validationErrors.Add(": 服务产品的星级不能大于商家的综合星级：" + ComprehensiveStar);
                return false;
            }
            var products = GetMerchantServiceProduct(entity.MerchantId);
            var stars = products.Sum(o => o.Star);

            var totalStars = 0;
            if (action=="create")
            {
                totalStars=stars + entity.Star;
            }
            if (action=="edit")
            {
                int star = GetById(entity.Id).Star;
                 totalStars=stars-star + entity.Star;
            }
            if (totalStars > ComprehensiveStar)
            {
                validationErrors.Add(@": 所有产品星级数之和不能大于商家的星级: "+ComprehensiveStar);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取商家Id获取商家用户信息
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        private string GetMerchantPerson(string merchantId)
        {
            string merchant = string.Empty;
            using (var dataContext = new SysEntities())
            {
                string sql = @"
                    SELECT  SysPersonId,
                            MerchantId
                    FROM    MerchantSysPerson MP
                    WHERE   MP.MerchantId = @MerchantId";
                SqlCommand scmd = new SqlCommand(sql);
                scmd.Parameters.AddWithValue("@MerchantId", merchantId);
                var dataSource = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(dataContext, scmd).Tables[0];
                if (dataSource.Rows.Count<=0)
                {
                    merchant = "";
                }
                else
                {
                    foreach (DataRow item in dataSource.Rows)
                    {
                        merchant = item["MerchantId"].ToString();
                    }
                }

                return merchant;
            }
        }

        /// <summary>
        /// 创建一个服务产品
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="db">数据库上下文</param>
        /// <param name="entity">一个服务产品</param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, SysEntities db, ServiceProduct entity)
        {
            using (var repository = new ServiceProductRepository())
            {
                try
                {
                    if (CheckName(entity))
                    {
                        validationErrors.Add("服务产品名称已被占用，请换一个新的服务产品名称");
                        return false;
                    }
                    if (string.IsNullOrEmpty(entity.MerchantId))
                    {

                        validationErrors.Add("： 请选择产品所属的商家！");
                        return false;
                    }
                    var createPersonId = GetMerchantPerson(entity.MerchantId);
                    if (string.IsNullOrEmpty(createPersonId))
                    {
                       validationErrors.Add(@"： 此商家还没有分配登录用户，不能为其添加服务产品，若需要为其添加服务产品请
                                    先给该商家分配登录用户！");
                        return false;
                    }
                    if (entity.Star != 0)
                    {
                        if (!StarValidation(validationErrors,entity,"create"))
                        {
                            return false;
                        }
                        else
                        {
                            repository.Create(db, entity);
                            repository.Save(db);
                            return true;
                        }
                    }
                    else
                    {
                        repository.Create(db, entity);
                        repository.Save(db);
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
        /// 创建服务产品
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, ServiceProduct entity)
        {
            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (Create(ref validationErrors,db, entity))
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
        /// 创建一个服务产品
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="db">数据库上下文</param>
        /// <param name="entity">一个服务产品</param>
        /// <returns></returns>
        public bool Delete(ref ValidationErrors validationErrors, int id)
        {
            using (var repository = new ServiceProductRepository())
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
        /// 删除服务产品集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="deleteCollection">主键的服务产品</param>
        /// <returns></returns>    
        public bool DeleteCollection(ref ValidationErrors validationErrors, string[] deleteCollection)
        {
            using (var repository = new ServiceProductRepository())
            {
                try
                {
                    if (deleteCollection != null)
                    {
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
        ///  创建服务产品集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entitys">服务产品集合</param>
        /// <returns></returns>
        public bool EditCollection(ref ValidationErrors validationErrors, IQueryable<ServiceProduct> entitys)
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
                                if (Edit(ref validationErrors, entity))
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
        /// 编辑一个服务产品
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个数据字典</param>
        /// <returns></returns>
        public bool Edit(ref ValidationErrors validationErrors, SysEntities db, ServiceProduct entity)
        {
            using (var repository = new ServiceProductRepository())
            {
                try
                {
                    if (CheckName(entity))
                    {
                        validationErrors.Add("服务产品名称已被占用，请换一个新的服务产品名称");
                        return false;
                    }
                    else if (entity.Star != 0)
                    {

                        if (!StarValidation(validationErrors, entity,"edit"))
                        {
                            return false;
                        }
                        else
                        {
                            repository.Edit(db, entity);
                            repository.Save(db);
                            return true;
                        }
                    }
                    else
                    {
                        repository.Edit(db, entity);
                        repository.Save(db);
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
        /// 编辑一个服务产品
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个服务产品</param>
        /// <returns>是否编辑成功</returns>
        public bool Edit(ref ValidationErrors validationErrors,  ServiceProduct entity)
        {
            try
            {
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

        /// <summary>
        /// 获取所有服务产品
        /// </summary>
        /// <returns></returns>
        public List<ServiceProduct> GetAll()
        {
            using (var repository = new ServiceProductRepository())
            {
                return repository.GetAll(db).ToList();
            }
        }

        /// <summary>
        /// 根据主键获取一个服务产品
        /// </summary>
        /// <param name="id">服务产品的主键</param>
        /// <returns>一个服务产品</returns>
        public ServiceProduct GetById(int id)
        {
            using (var repository = new ServiceProductRepository())
            {
                return repository.GetById(db, id);
            }
        }

        public void Dispose()
        {

        }
    }
}
