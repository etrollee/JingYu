using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace DAL
{
    /// <summary>
    /// 服务产品
    /// </summary>
    public class ServiceProductRepository : BaseRepository<ServiceProduct>, IDisposable
    {
        /// <summary>
        /// 查询的数据
        /// </summary>
        /// <param name="SysEntities">数据访问的上下文</param>
        /// <param name="order">排序字段</param>
        /// <param name="sort">升序asc（默认）还是降序desc</param>
        /// <param name="search">查询条件</param> 
        /// <param name="listQuery">额外的参数</param>
        /// <returns></returns>    
        public IQueryable<ServiceProduct> DaoChuData(SysEntities db, string order, string sort,
            string search, params object[] listQuery)
        {
            string where = string.Empty;
            int flagWhere = 0;
            Dictionary<string, string> queryDic = ValueConvert.StringToDictionary(search.GetString());
            if (queryDic != null && queryDic.Count > 0)
            {

                foreach (var item in queryDic)
                {
                    if (flagWhere != 0)
                    {
                        where += " and ";
                    }
                    flagWhere++;
                    if (queryDic.ContainsKey("Merchant") && !string.IsNullOrWhiteSpace(item.Key) &&
                        !string.IsNullOrWhiteSpace(item.Value) && item.Key == "Merchant")
                    {
                        where += "EXISTS(select p from it.Merchant as p where p.id='" + item.Value + ")";
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value)
                        && item.Key.Contains(Start_Time)) //需要查询的列名
                    {
                        where += "it. " + item.Key.Remove(item.Key.IndexOf(Start_Time)) +
                            " >=  CAST('" + item.Value + "' as   System.DateTime)";
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value)
                        && item.Key.Contains(End_Time)) //需要查询的列名
                    {
                        where += "it." + item.Key.Remove(item.Key.IndexOf(End_Time)) +
                            " <  CAST('" + Convert.ToDateTime(item.Value).AddDays(1) + "' as   System.DateTime)";
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value)
                        && item.Key.Contains(Start_Int)) //需要查询的列名
                    {
                        where += "it." + item.Key.Remove(item.Key.IndexOf(Start_Int)) + " >= " + item.Value.GetInt();
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value)
                        && item.Key.Contains(End_Int)) //需要查询的列名
                    {
                        where += "it." + item.Key.Remove(item.Key.IndexOf(End_Int)) + " <= " + item.Value.GetInt();
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value)
                        && item.Key.Contains(End_String)) //需要查询的列名
                    {
                        where += "it." + item.Key.Remove(item.Key.IndexOf(End_String)) + " = '" + item.Value + "'";
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(item.Key) && !string.IsNullOrWhiteSpace(item.Value)
                        && item.Key.Contains(DDL_String)) //需要查询的列名
                    {
                        where += "it." + item.Key.Remove(item.Key.IndexOf(DDL_String)) + " = '" + item.Value + "'";
                        continue;
                    }
                    where += "it." + item.Key + " like '%" + item.Value + "%'";
                }
            }
            return db.ServiceProduct.Where(string.IsNullOrEmpty(where) ? "true" : where)
                .OrderBy("it." + sort.GetString() + " " + order.GetString())
                .AsQueryable();
        }

        /// <summary>
        /// 通过主键id，获取服务产品---查看详细，首次编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>服务产品</returns>
        public ServiceProduct GetById(SysEntities db, int id)
        {
            return db.ServiceProduct.SingleOrDefault(o => o.Id == id);
        }

        /// <summary>
        /// 通过主键id，获取服务产品---查看详细，首次编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>服务产品</returns>
        public ServiceProduct GetById(int id)
        {
            using (SysEntities db = new SysEntities())
            {
                return GetById(db, id);
            }
        }

        public void Delete(SysEntities db, int id)
        {
            ServiceProduct deleteItem = GetById(db, id);
            if (deleteItem != null)
            {
                db.ServiceProduct.DeleteObject(deleteItem);
            }
        }

        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="db">实体数据</param>
        /// <param name="deleteCollection">主键的集合</param>
        public void Delete(SysEntities db, string[] deleteCollection)
        {

            //数据库设置级联关系，自动删除子表的内容   
             var temp = deleteCollection.ToIntList();
            IQueryable<ServiceProduct> collection = from f in db.ServiceProduct
                                                 where temp.Contains(f.Id)
                                                 select f;
            foreach (var deleteItem in collection)
            {
                db.ServiceProduct.DeleteObject(deleteItem);
            }
        }

        /// <summary>
        /// 确定删除一个对象，调用Save方法
        /// </summary>
        /// <param name="id">一条数据的主键</param>
        /// <returns></returns>    
        public int Delete(int id)
        {
            using (SysEntities db = new SysEntities())
            {
                this.Delete(db, id);
                return Save(db);
            }
        }


        public void Dispose()
        { }
    }
}
