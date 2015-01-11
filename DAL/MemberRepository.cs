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
    /// huiyuan
    /// </summary>
    public class MemberRepository : BaseRepository<Member>, IDisposable
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
        public IQueryable<Member> DaoChuData(SysEntities db, string order, string sort,
            string search, string personId, params object[] listQuery)
        {
            List<Member> list = new List<Member>();
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
                    if (queryDic.ContainsKey("MemberGroupId") && !string.IsNullOrWhiteSpace(item.Key) &&
                      !string.IsNullOrWhiteSpace(item.Value) && item.Key == "MemberGroupId")
                    {
                        where += "it.Id IN(select MemberId from Member_MemberGroup as p where p.MemberGroupId='" + item.Value + "')";
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
            try
            {
                string sql = @"
                    SELECT*FROM(SELECT  MB.Id ,
                            MB.Name ,
                            MB.Code ,
                            CreatePersonId ,
                            MB.RegisterCode ,
                            Area ,
                            Regtime ,
                            RegisteredCapital ,
                            MB.Contacts ,
                            Phone ,
                            RegisteredAddress ,
                            LegalRepresentative ,
                            RegisteredCellPhone ,
                            SelfDefineOne ,
                            SelfDefineTwo ,
                            Remark ,
                            VIP ,
                            IsValid ,
                            MB.IsVisible,
                            LogOnTimes ,
                            MC.Name AS MerchantName,
                            MC.ParentId
                    FROM    Member MB
                            LEFT JOIN MerchantSysPerson MSP ON MB.CreatePersonId = MSP.SysPersonId
                            LEFT JOIN Merchant MC ON MSP.MerchantId = MC.Id
                                        {0} )it
                                        {1}
                                        ORDER BY it.ParentId,it.Id DESC ";
                string selectSql = "WHERE 1=1";
                if (!string.IsNullOrEmpty(personId))
                {
                    string whereString = @"
                     AND CreatePersonId = @SysPersonId
                            OR CreatePersonId IN (
                            SELECT  SysPersonId
                            FROM    MerchantSysPerson
                            WHERE   MerchantId IN (
                                    SELECT  Id
                                    FROM    Merchant
                                    WHERE   ParentId = ( SELECT MerchantId
                                                         FROM   MerchantSysPerson
                                                         WHERE  SysPersonId =@SysPersonId
                                                       ) ) )";
                    selectSql = string.Concat(selectSql, whereString);
                }
                string selectSecondSql = "WHERE 1=1";
                if (queryDic.Count > 0)
                {
                    selectSecondSql = string.Concat(selectSecondSql, " AND " + where);
                }
                sql = string.Format(sql, selectSql, selectSecondSql);
                SqlCommand scmd = new SqlCommand(sql);
                if (!string.IsNullOrEmpty(personId))
                {
                    scmd.Parameters.AddWithValue("@SysPersonId", personId);
                }
                var dataSources = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd).Tables[0];
                foreach (DataRow item in dataSources.Rows)
                {
                    var member = new Member();
                    member.Id = item["Id"].ToString();
                    member.Name = item["Name"].ToString();
                    member.Code = item["Code"].ToString();
                    member.CreatePersonId = item["CreatePersonId"] == null ? "" : item["CreatePersonId"].ToString();
                    member.RegisterCode = item["RegisterCode"] == null ? "" : item["RegisterCode"].ToString();
                    member.Area = item["Area"] == null ? "" : item["Area"].ToString();
                    member.Regtime = Convert.IsDBNull(item["Regtime"]) ? DateTime.Now : Convert.ToDateTime(item["Regtime"]);
                    member.RegisteredCapital = Convert.IsDBNull(item["RegisteredCapital"]) ? 0 :
                        Convert.ToDecimal(item["RegisteredCapital"]);
                    member.Contacts = item["Contacts"] == null ? "" : item["Contacts"].ToString();
                    member.Phone = item["Phone"] == null ? "" : item["Phone"].ToString();
                    member.RegisteredAddress = item["RegisteredAddress"] == null ? "" : item["RegisteredAddress"].ToString();
                    member.LegalRepresentative = item["LegalRepresentative"] == null ? "" : item["LegalRepresentative"].ToString();
                    member.RegisteredCellPhone = item["RegisteredCellPhone"] == null ? "" : item["RegisteredCellPhone"].ToString();
                    member.SelfDefineOne = item["SelfDefineOne"] == null ? "" : item["SelfDefineOne"].ToString();
                    member.SelfDefineTwo = item["SelfDefineTwo"] == null ? "" : item["SelfDefineTwo"].ToString();
                    member.Remark = item["Remark"] == null ? "" : item["Remark"].ToString();
                    member.VIP = Convert.ToBoolean(item["VIP"]);
                    member.IsValid = Convert.ToBoolean(item["IsValid"]);
                    member.IsVisible = Convert.ToBoolean(item["IsVisible"]);
                    member.LogOnTimes = Convert.ToInt32(item["LogOnTimes"]);

                    list.Add(member);
                }

                return list.AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 通过主键id，获取会员---查看详细，首次编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>会员</returns>
        public Member GetById(SysEntities db, string id)
        {
            return db.Member.SingleOrDefault(o => o.Id == id);
        }

        /// <summary>
        /// 通过主键id，获取会员---查看详细，首次编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>会员</returns>
        public Member GetById(string id)
        {
            using (SysEntities db = new SysEntities())
            {
                return GetById(db, id);
            }
        }

        public void Delete(SysEntities db, string id)
        {
            Member deleteItem = GetById(db, id);
            if (deleteItem != null)
            {
                db.Member.DeleteObject(deleteItem);
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
            //var temp = deleteCollection.ToIntList();
            IQueryable<Member> collection = from f in db.Member
                                            where deleteCollection.Contains(f.Id)
                                            select f;
            foreach (var deleteItem in collection)
            {
                db.Member.DeleteObject(deleteItem);
            }
        }




        /// <summary>
        /// 确定删除一个对象，调用Save方法
        /// </summary>
        /// <param name="id">一条数据的主键</param>
        /// <returns></returns>    
        public int Delete(string id)
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
