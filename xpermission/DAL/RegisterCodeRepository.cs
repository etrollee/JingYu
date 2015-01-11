using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Common;

namespace DAL
{
    /// <summary>
    /// 注册码
    /// </summary>
    public class RegisterCodeRepository : BaseRepository<RegisterCode>, IDisposable
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
        public IQueryable<RegisterCode> DaoChuData(SysEntities db, string order, string sort,
            string search,string merchantId, params object[] listQuery)
        {
            List<RegisterCode> list = new List<RegisterCode>();
            string where = string.Empty;
            int flagWhere = 0;
            Dictionary<string, string> queryDic= ValueConvert.StringToDictionary(search.GetString());
            if (queryDic != null && queryDic.Count > 0)
            {

                foreach (var item in queryDic)
                {
                    if (flagWhere != 0)
                    {
                        where += " and ";
                    }
                    flagWhere++;
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
            try
            {
                string sql = @"
                        SELECT*FROM(SELECT  RC.Id ,
                                Value ,
                                IsUsed ,
                                IsDistribution ,
                                RC.IsValid ,
                                RC.BelongMerchantId,
		                        MT.Name AS BelongMerchant,
                                M.Name AS MerchantName ,
                                Mem.Name AS MemberName,
                                RC.CreateTime
                        FROM    RegisterCode RC
                                LEFT JOIN Merchant M ON RC.MerchantId = M.Id
                                 LEFT JOIN Merchant MT ON RC.BelongMerchantId = MT.Id
                                LEFT JOIN Member Mem ON RC.MemberId = Mem.Id)it
                        {0}
                        ORDER BY it.CreateTime DESC";
                string selectSql = "WHERE 1=1";
                if (queryDic.Count>0)
                {
                    selectSql = string.Concat(selectSql," AND "+where);
                }
                if (!string.IsNullOrEmpty(merchantId))
                {
                    selectSql = string.Concat(selectSql, " AND it.BelongMerchantId='"+merchantId+"'");
                }
                sql = string.Format(sql,selectSql);
                SqlCommand scmd = new SqlCommand(sql);

                var dataSources = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd).Tables[0];
                foreach (DataRow item in dataSources.Rows)
                {
                    var registerCode = new RegisterCode();
                    registerCode.Id = Convert.ToInt32(item["Id"]);
                    registerCode.Value = item["Value"].ToString();
                    registerCode.CreateTime = Convert.ToDateTime(item["CreateTime"]);
                    registerCode.BelongMerchant = item["BelongMerchant"].ToString();
                    registerCode.MerchantName = item["MerchantName"].ToString();
                    registerCode.MemberName = item["MemberName"].ToString();
                    registerCode.IsValid = Convert.ToBoolean(item["IsValid"]);
                    registerCode.IsUsed = Convert.ToBoolean(item["IsUsed"]);
                    registerCode.IsDistribution = Convert.ToBoolean(item["IsDistribution"]);
                    list.Add(registerCode);
                }

                return list.AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 查询的数据
        /// </summary>
        /// <param name="SysEntities">数据访问的上下文</param>
        /// <param name="order">排序字段</param>
        /// <param name="sort">升序asc（默认）还是降序desc</param>
        /// <param name="search">查询条件</param> 
        /// <param name="listQuery">额外的参数</param>
        /// <returns></returns>    
        public IQueryable<RegisterCode> GetMerchantRegisterCodes(SysEntities db, string merchantId,
            string search, params object[] listQuery)
        {
            List<RegisterCode> list = new List<RegisterCode>();
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
            try
            {
                string sql = @"
                        SELECT*FROM(SELECT  RC.Id ,
                                Value ,
                                IsUsed ,
                                IsDistribution ,
                                RC.IsValid ,
		                        MT.Name AS BelongMerchant,
                                M.Name AS MerchantName ,
                                Mem.Name AS MemberName
                        FROM    RegisterCode RC
                                LEFT JOIN Merchant M ON RC.MerchantId = M.Id
                                 LEFT JOIN Merchant MT ON RC.BelongMerchantId = MT.Id
                                LEFT JOIN Member Mem ON RC.MemberId = Mem.Id)it
                        {0}";
                string selectSql = "WHERE RC.BelongMerchantId=@MerchantId";
                if (queryDic.Count > 0)
                {
                    selectSql = string.Concat(selectSql, " AND " + where);
                }
                sql = string.Format(sql, selectSql);
                SqlCommand scmd = new SqlCommand(sql);
                scmd.Parameters.AddWithValue("@MerchantId",merchantId);
                var dataSources = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd).Tables[0];
                foreach (DataRow item in dataSources.Rows)
                {
                  var registerCode = new RegisterCode();
                    registerCode.Id = Convert.ToInt32(item["Id"]);
                    registerCode.Value = item["Value"].ToString();
                    registerCode.BelongMerchant = item["BelongMerchant"].ToString();
                    registerCode.MerchantName = item["MerchantName"].ToString();
                    registerCode.MemberName = item["MemberName"].ToString();
                    registerCode.IsValid = Convert.ToBoolean(item["IsValid"]);
                    registerCode.IsUsed = Convert.ToBoolean(item["IsUsed"]);
                    registerCode.IsDistribution = Convert.ToBoolean(item["IsDistribution"]);
                    list.Add(registerCode);
                }

                return list.AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 查询的数据
        /// </summary>
        /// <param name="SysEntities">数据访问的上下文</param>
        /// <param name="order">排序字段</param>
        /// <param name="sort">升序asc（默认）还是降序desc</param>
        /// <param name="search">查询条件</param> 
        /// <param name="listQuery">额外的参数</param>
        /// <returns></returns>    
        public IQueryable<RegisterCode> GetRegisterCodeLogin(SysEntities db, string order, string sort,
            string search,  params object[] listQuery)
        {
            List<RegisterCode> list = new List<RegisterCode>();
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
            try
            {
                string sql = @"
                        SELECT*FROM(
                        SELECT  RCL.Id ,
                                RCL.RCode AS Value,
                                M.Name AS MemberName ,
                                MC.Name AS MerchantName ,
                                LoginTime ,
                                SerialPort
                        FROM    dbo.RegisterCodeLogin RCL
                                INNER JOIN dbo.RegisterCode RC ON RCL.RCode = RC.Value
                                LEFT JOIN dbo.Member M ON RC.MemberId = M.Id
                                LEFT JOIN Merchant MC ON RC.MerchantId = MC.Id)it
                        {0}";
                string selectSql = "WHERE 1=1";
                if (queryDic.Count > 0)
                {
                    selectSql = string.Concat(selectSql, " AND " + where);
                }
                sql = string.Format(sql, selectSql);
                SqlCommand scmd = new SqlCommand(sql);
                var dataSources = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd).Tables[0];
                foreach (DataRow item in dataSources.Rows)
                {
                    var registerCode = new RegisterCode();
                    registerCode.Id = Convert.ToInt32(item["Id"]);
                    registerCode.Value = item["Value"].ToString();
                    registerCode.MerchantName = item["MerchantName"].ToString();
                    registerCode.MemberName = item["MemberName"].ToString();
                    registerCode.LoginTime =Convert.ToDateTime(item["LoginTime"]);
                    registerCode.SerialPort = item["SerialPort"].ToString();
                    list.Add(registerCode);
                }

                return list.AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 通过主键id，获取注册码---查看详细，首次编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>注册码</returns>
        public RegisterCode GetById(SysEntities db, int id)
        {
            var registerCode = new RegisterCode();

            try
            {
                string sql = @"
                    SELECT  RC.Id ,
                            Value ,
                            IsUsed ,
                            IsDistribution ,
                            RC.IsValid ,
                            RC.CreateTime,
                            RC.BelongMerchantId,
                            RC.MerchantId,
                            RC.MemberId,
                            MT.Name AS BelongMerchant ,
                            M.Name ,
                            M.Name AS MerchantName ,
                            Mem.Name AS MemberName
                    FROM    RegisterCode RC
                            LEFT JOIN Merchant M ON RC.MerchantId = M.Id
                            LEFT JOIN Merchant MT ON RC.BelongMerchantId = MT.Id
                            LEFT JOIN Member Mem ON RC.MemberId = Mem.Id
                            WHERE RC.Id=@RegisterCodeId";
                SqlCommand scmd = new SqlCommand(sql);
                scmd.Parameters.AddWithValue("@RegisterCodeId", id);
                var dataSources = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd).Tables[0];
                foreach (DataRow item in dataSources.Rows)
                {
                    registerCode.Id = Convert.ToInt32(item["Id"]);
                    registerCode.Value = item["Value"].ToString();
                    registerCode.CreateTime = Convert.ToDateTime(item["CreateTime"]);
                    registerCode.MerchantId = item["MerchantId"].ToString();
                    registerCode.MemberId = item["MemberId"].ToString();
                    registerCode.BelongMerchantId = item["BelongMerchantId"].ToString();
                    registerCode.Id = Convert.ToInt32(item["Id"]);
                    registerCode.BelongMerchant = item["BelongMerchant"].ToString();
                    registerCode.MerchantName = item["MerchantName"].ToString();
                    registerCode.Name = item["Name"].ToString();
                    registerCode.MemberName = item["MemberName"].ToString();
                    registerCode.IsValid = Convert.ToBoolean(item["IsValid"]);
                    registerCode.IsUsed = Convert.ToBoolean(item["IsUsed"]);
                    registerCode.IsDistribution = Convert.ToBoolean(item["IsDistribution"]);
                }

                return registerCode;
            }
            catch (Exception)
            {
                
                throw;
            }
          //  return db.RegisterCode.SingleOrDefault(o => o.Id == id);
        }

        /// <summary>
        /// 根据注册码获取注册码信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public RegisterCode GetByValue(SysEntities db, string value)
        {
            return db.RegisterCode.SingleOrDefault(o=>o.Value==value);
        }

        /// <summary>
        /// 通过主键id，获取注册码---查看详细，首次编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>注册码</returns>
        public RegisterCode GetById(int id)
        {
            using (SysEntities db = new SysEntities())
            {
                return GetById(db, id);
            }
        }

        /// <summary>
        /// 删除一个注册码
        /// </summary>
        /// <param name="db">实体数据</param>
        /// <param name="id">一条人员的主键</param>
        public void Delete(SysEntities db, int id)
        {
            RegisterCode deleteItem = GetById(db, id);
            if (deleteItem != null)
            {
                db.RegisterCode.DeleteObject(deleteItem);
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
            IQueryable<RegisterCode> collection = from f in db.RegisterCode
                                                  where temp.Contains(f.Id)
                                                  select f;
            foreach (var deleteItem in collection)
            {
                db.RegisterCode.DeleteObject(deleteItem);
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
