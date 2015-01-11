using System;
using System.Collections.Generic;
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
    public class AppointmentBLL : IAppointmentBLL, IDisposable
    {
        /// <summary>
        /// 私有的数据访问上下文
        /// </summary>
        protected SysEntities db;
        ISysPersonBLL _iSysPersonBll;

        public AppointmentBLL()
            : this(new SysPersonBLL())
        {
        }

        /// <summary>
        /// 构造函数，默认加载数据访问上下文
        /// </summary>
        public AppointmentBLL(SysPersonBLL sysPersonBll)
        {
            _iSysPersonBll = sysPersonBll;
            db = new SysEntities();
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
        public List<Appointment> GetByParam(string sysPersonId, int id, int page, int rows, string order,
              string sort, string search, ref int total)
        {
            using (var repository = new AppointmentRepository())
            {
                IQueryable<Appointment> queryData = null;
                var sysRole = _iSysPersonBll.GetRefSysRole(sysPersonId).FirstOrDefault();
                var merchant = _iSysPersonBll.GetRefMerchant(sysPersonId).FirstOrDefault();
                if (sysRole.Power == 1 || sysRole.Power == 2)
                {
                    queryData = repository.DaoChuData(db, order, sort, search);
                }
                else
                {
                    queryData = repository.DaoChuData(db, order, sort, search)
                        .Where(o => o.ServiceProduct.MerchantId == merchant.Id);
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
        /// 编辑一个对象
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个对象</param>
        /// <returns></returns>
        public bool Edit(ref ValidationErrors validationErrors, Appointment entity)
        {
            try
            {
                string sql = @"UPDATE Appointment SET State=@State WHERE Id=@Id";
                var args = new DbParameter[] 
                    {
                        new SqlParameter{ParameterName="State",Value=entity.State},
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

        /// <summary>
        /// 删除一个预约
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(ref ValidationErrors validationErrors, int id)
        {
            using (var repository = new AppointmentRepository())
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
        /// 删除预约集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="deleteCollection">主键的预约</param>
        /// <returns></returns>    
        public bool DeleteCollection(ref ValidationErrors validationErrors, string[] deleteCollection)
        {
            using (var repository = new AppointmentRepository())
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
        /// 根据主键获取一个预约
        /// </summary>
        /// <param name="id">反馈模板的主键</param>
        /// <returns>一个反馈模板</returns>
        public Appointment GetById(int id)
        {
            using (var repository = new AppointmentRepository())
            {
                return repository.GetById(db, id);
            }
        }

        public void Dispose()
        {

        }
    }
}
