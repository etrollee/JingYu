using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Common;
using DAL;
using IBLL;

namespace BLL
{
    public class MerchantTypeBLL:IMerchantTypeBLL,IDisposable
    {
        /// <summary>
        /// 私有的数据访问上下文
        /// </summary>
        protected SysEntities db;

             /// <summary>
        /// 构造函数，默认加载数据访问上下文
        /// </summary>
        public MerchantTypeBLL()
        {
            db = new SysEntities();
        }

        /// <summary>
        /// 验证商家类型是否存在
        /// </summary>
        /// <param name="name">商家类型</param>
        /// <returns></returns>
        public bool CheckName(MerchantType entity)
        {
            return db.MerchantType.Any(o => o.Name == entity.Name && o.Id != entity.Id);
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
        public List<MerchantType> GetByParam(int id, int page, int rows, string order,
            string sort, string search, ref int total)
        {

            using (var repository = new MerchantTypeRepository())
            {
                IQueryable<MerchantType> queryData = repository.DaoChuData(db, order, sort, search);
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

        public bool Create(ref ValidationErrors validationErrors, MerchantType entity)
        {
            using (var repository = new MerchantTypeRepository())
            {
                try
                {
                    if (CheckName(entity))
                    {
                        validationErrors.Add("类型名称已被占用，请换一个新的类型名称");
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

        public bool Delete(ref ValidationErrors validationErrors, int id)
        {
            using (var repository = new MerchantTypeRepository())
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
        /// 删除商家类型集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="deleteCollection">主键的商家类型</param>
        /// <returns></returns>    
        public bool DeleteCollection(ref ValidationErrors validationErrors, string[] deleteCollection)
        {
            using (var repository = new MerchantTypeRepository())
            {
                try
                {
                    if (deleteCollection != null)
                    {

                        using (TransactionScope transactionScope = new TransactionScope())
                        {
                            var temp = deleteCollection.ToIntList();
                            IQueryable<MerchantType> collection = from f in db.MerchantType
                                                                  where temp.Contains(f.Id)
                                                                  select f;
                            foreach (var deleteItem in collection)
                            {
                                var merchant=db.Merchant.SingleOrDefault(o=>o.MerchantTypeId==deleteItem.Id);
                                if (merchant!=null)
                                {
                                    validationErrors.Add("该商家类型正被使用，不可删除,若要删除请先先删除该类型下的商家");
                                    return false;
                                }
                            }
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
        /// 编辑一个商家类别
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个数据字典</param>
        /// <returns></returns>
        public bool Edit(ref ValidationErrors validationErrors, MerchantType entity)
        {
            using (var repository = new MerchantTypeRepository())
            {
                try
                {
                    if (CheckName(entity))
                    {
                        validationErrors.Add("类型名称已被占用，请换一个新的类型名称");
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
        /// 获取所有商家类别
        /// </summary>
        /// <returns></returns>
        public List<MerchantType> GetAll()
        {
            using (var repository = new MerchantTypeRepository())
            {
                return repository.GetAll(db).ToList();
            }
        }

        /// <summary>
        /// 根据主键获取一个商家类型
        /// </summary>
        /// <param name="id">商家类型的主键</param>
        /// <returns>一个商家类型</returns>
        public MerchantType GetById(int id)
        {
            using (var repository = new MerchantTypeRepository())
            {
                return repository.GetById(db, id);
            }
        }

        public void Dispose()
        {

        }
    }
}
