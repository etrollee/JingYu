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
    public class FeedbackTemplateBLL : IFeedbackTemplateBLL, IDisposable
    {
        /// <summary>
        /// 私有的数据访问上下文
        /// </summary>
        protected SysEntities db;

        /// <summary>
        /// 构造函数，默认加载数据访问上下文
        /// </summary>
        public FeedbackTemplateBLL()
        {
            db = new SysEntities();
        }

        /// <summary>
        /// 验证反馈模板名称是否存在
        /// </summary>
        /// <param name="name">反馈模板</param>
        /// <returns></returns>
        public bool CheckName(FeedbackTemplate entity)
        {
            return db.FeedbackTemplate.Any(o => o.CreatePersonId == entity.CreatePersonId
                && o.Name == entity.Name && o.Id != entity.Id);
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
        public List<FeedbackTemplate> GetByParam(string sysPersonId, string id, int page, int rows, string order,
              string sort, string search, ref int total)
        {
            using (var repository = new FeedbackTemplateRepository())
            {

                IQueryable<FeedbackTemplate> queryData = repository.DaoChuData(db, order, sort, search)
                    .Where(o => o.CreatePersonId == sysPersonId || o.IsSys);
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
        /// 创建反馈模块
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, FeedbackTemplate entity)
        {
            using (var repository = new FeedbackTemplateRepository())
            {
                try
                {

                    var template = db.FeedbackTemplate.SingleOrDefault(o => o.IsSys && o.Name == entity.Name);
                    if (template != null)
                    {
                        validationErrors.Add("： 自定义的模板内容不能与系统默认的模板内容相同！");
                        return false;
                    }
                    if (CheckName(entity))
                    {
                        validationErrors.Add("： 模板内容已被占用，请换一个新的模板内容");
                        return false;
                    }
                    var count = GetAll().Where(o => o.CreatePersonId == entity.CreatePersonId).Count();
                    if (count == 2)
                    {
                        validationErrors.Add("： 你已经自定义了2个版本的态度反馈模板,不能再定义了！");
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
        /// 删除反馈模块
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(ref ValidationErrors validationErrors, string id)
        {
            using (var repository = new FeedbackTemplateRepository())
            {
                try
                {
                    int count = GetInfoFackbackTemplateCount(id);
                    if (count > 0)
                    {
                        validationErrors.Add("模版正在使用，不可删除，请先修改正在使用该信息的模版!");
                        return false;
                    }
                    else
                    {
                        return repository.Delete(id) == 1;
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
        /// 删除反馈模板集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="deleteCollection">主键的反馈模板</param>
        /// <returns></returns>    
        public bool DeleteCollection(ref ValidationErrors validationErrors, string[] deleteCollection)
        {
            using (var repository = new FeedbackTemplateRepository())
            {
                try
                {
                    int count = 0;
                    if (deleteCollection != null)
                    {
                        //数据库设置级联关系，自动删除子表的内容   
                        //var temp = deleteCollection.ToIntList();
                        IQueryable<FeedbackTemplate> collection = from f in db.FeedbackTemplate
                                                                  where deleteCollection.Contains(f.Id)
                                                                  select f;
                        foreach (var deleteItem in collection)
                        {
                            count = GetInfoFackbackTemplateCount(deleteItem.Id);
                            if (count > 0)
                            {
                                validationErrors.Add("模版正在使用，不可删除，请先修改正在使用该信息的模版!");
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
        /// 编辑一个反馈模板
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个数据字典</param>
        /// <returns></returns>
        public bool Edit(ref ValidationErrors validationErrors, FeedbackTemplate entity)
        {
            using (var repository = new FeedbackTemplateRepository())
            {
                try
                {
                    var template = db.FeedbackTemplate.SingleOrDefault(o => o.IsSys && o.Name == entity.Name);
                    if (template != null)
                    {
                        validationErrors.Add("： 自定义的模板内容不能与系统默认的模板内容相同！");
                        return false;
                    }
                    if (CheckName(entity))
                    {
                        validationErrors.Add(": 模板内容已被占用，请换一个新的模板内容");
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

        public List<FeedbackTemplate> GetAll()
        {
            using (var repository = new FeedbackTemplateRepository())
            {
                return repository.GetAll(db).ToList();
            }
        }


        /// <summary>
        /// 根据主键获取一个反馈模板
        /// </summary>
        /// <param name="id">反馈模板的主键</param>
        /// <returns>一个反馈模板</returns>
        public FeedbackTemplate GetById(string id)
        {
            using (var repository = new FeedbackTemplateRepository())
            {
                return repository.GetById(db, id);
            }
        }

        /// <summary>
        /// 获取某反馈模板信息使用数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetInfoFackbackTemplateCount(string id)
        {
            using (var dataContext = new SysEntities())
            {
                int count = 0;
                string sql = @"
                    SELECT  COUNT(FeedbackTemplateId) AS Count
                    FROM    dbo.InformationFeedbackTemplate
                    WHERE   FeedbackTemplateId = @TemplateId";
                SqlCommand scmd = new SqlCommand(sql);
                scmd.Parameters.AddWithValue("@TemplateId", id);
                var data = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(dataContext, scmd).Tables[0];
                foreach (DataRow item in data.Rows)
                {
                    count = Convert.ToInt32(item["Count"]);
                }

                return count;
            }
        }

        public void Dispose()
        {

        }
    }
}
