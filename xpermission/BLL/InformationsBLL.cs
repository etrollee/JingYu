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
    public class InformationsBLL : IInformationsBLL, IDisposable
    {
        /// <summary>
        /// 私有的数据访问上下文
        /// </summary>
        protected SysEntities db;

        public InformationsBLL()
            : this(new SysPersonBLL())
        {
        }

        ISysPersonBLL _iSysPersonBll;
        /// <summary>
        /// 构造函数，默认加载数据访问上下文
        /// </summary>
        public InformationsBLL(SysPersonBLL sysPersonBll)
        {
            _iSysPersonBll = sysPersonBll;
            db = new SysEntities();
        }

        /// <summary>
        /// 验证信息标题是否存在
        /// </summary>
        /// <param name="entity">信息实体</param>
        /// <returns></returns>
        public bool CheckTitle(Informations entity)
        {
            return db.Informations.Any(o => o.Title == entity.Title.Trim()
                && o.CreatePersonId == entity.CreatePersonId && o.Id != entity.Id);
        }

        /// <summary>
        /// 获取会员
        /// </summary>
        /// <returns></returns>
        protected List<Member> GetMembers()
        {
            using (var dataContext = new SysEntities())
            {
                return dataContext.Member.ToList();
            }
        }

        /// <summary>
        /// 获取商家
        /// </summary>
        /// <returns></returns>
        protected List<Merchant> GetMerchants()
        {
            using (var dataContext = new SysEntities())
            {
                return dataContext.Merchant.ToList();
            }
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
        public List<Informations> GetByParam(string sysPersonId, string id, int page, int rows, string order,
            string sort, string search, ref int total)
        {
            using (var repository = new InformationsRepository())
            {
                IQueryable<Informations> queryData = null;
                var sysRole = _iSysPersonBll.GetRefSysRole(sysPersonId).FirstOrDefault();
                if (sysRole.Power == 1 || sysRole.Power == 2)
                {
                    queryData = repository.DaoChuData(db, order, sort, search);
                }
                else
                {
                    queryData = repository.DaoChuData(db, order, sort, search)
                        .Where(o => o.CreatePersonId == sysPersonId);
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
                        if (item.FeedbackTemplate != null)
                        {
                            item.FeedbackTemplateId = string.Empty;
                            foreach (var it in item.FeedbackTemplate)
                            {
                                item.FeedbackTemplateId += it.Name + ' ';
                            }
                        }
                        if (item.Member != null)
                        {
                            item.MemberId = string.Empty;
                            foreach (var it in item.Member)
                            {
                                item.MemberId += it.Name + ' ';
                            }
                        }
                    }
                }
                return queryData.ToList();
            }
        }

        /// <summary>
        /// 添加信息接收对象和使用的反馈模板
        /// </summary>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        private void AddInfoMemberAndFeedbackTemplate(SysEntities db, Informations entity)
        {
            //添加反馈模块
            foreach (string item in entity.FeedbackTemplateId.GetIdSort())
            {
                db = new SysEntities();
                string sql = @"INSERT INTO InformationFeedbackTemplate(InformationId,FeedbackTemplateId)
                                VALUES(@InformationId,@FeedbackTemplateId)";
                var args = new DbParameter[] 
                        { 
                             new SqlParameter { ParameterName = "InformationId", Value =entity.Id},
                             new SqlParameter { ParameterName = "FeedbackTemplateId", Value =item},
                        };
                db.ExecuteStoreCommand(sql, args);
            }
            //发送信息给会员
            foreach (string item in entity.MemberId.GetIdSort())
            {
                db = new SysEntities();
                string sql = @"INSERT INTO InformationMember(InformationId,MemberId)
                                VALUES(@InformationId,@MemberId)
                                INSERT INTO MemberInformations(InformationsId,MemberId)
                                                                VALUES(@InformationId,@MemberId)";
                var args = new DbParameter[] 
                        { 
                             new SqlParameter { ParameterName = "InformationId", Value =entity.Id},
                             new SqlParameter { ParameterName = "MemberId", Value =item},
                        };
                db.ExecuteStoreCommand(sql, args);
            }
            //发送信息给商家
            foreach (string item in entity.MerchantId.GetIdSort())
            {
                db = new SysEntities();
                string sql = @"INSERT INTO InformationMerchant(InformationId,MerchantId)
                              VALUES(@InformationId,@MerchantId)
                              INSERT INTO MerchantInformations(InformationsId,MerchantId)
                                                                VALUES(@InformationId,@MerchantId)";
                var args = new DbParameter[] 
                        {
                            new SqlParameter{ParameterName="InformationId",Value=entity.Id},
                            new SqlParameter{ParameterName="MerchantId",Value=item}
                        };
                db.ExecuteStoreCommand(sql,args);
            }
        }

        /// <summary>
        /// 添加会员信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="model"></param>
        private void AddToMemberInformations(SysEntities db, MemberInformations model)
        {
            string sql = @"INSERT INTO MemberInformations(InformationsId,MemberId)
                                VALUES(@InformationsId,@MemberId)";
            var args = new DbParameter[] 
                        { 
                             new SqlParameter { ParameterName = "InformationsId", Value =model.InformationsId},
                             new SqlParameter { ParameterName = "MemberId", Value =model.MemberId},
                        };
            db.ExecuteStoreCommand(sql, args);
        }

        /// <summary>
        /// 添加升级信息
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个对象</param>
        /// <returns></returns>
        public bool AddUpdateInfo(ref ValidationErrors validationErrors, SysEntities db, Informations entity)
        {
            using (var repository = new InformationsRepository())
            {
                try
                {
                    var members = GetMembers();
                    var merchants = GetMerchants();
                    repository.Create(entity);
                    if (members.Count > 0)
                    {
                        foreach (var item in members)
                        {
                            var memberInfo = new MemberInformations
                            {
                                InformationsId = entity.Id,
                                MemberId = item.Id,
                                IsRead = false,
                                IsFeedback = false
                            };
                            AddToMemberInformations(db, memberInfo);
                        }
                    }
                    if (merchants.Count > 0)
                    {
                        foreach (var item in merchants)
                        {
                            var merchantInfo = new MemberInformations
                            {
                                InformationsId = entity.Id,
                                MemberId = item.Id,
                                IsRead = false,
                                IsFeedback = false
                            };
                            AddToMemberInformations(db, merchantInfo);
                        }
                    }
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
        /// 添加升级信息
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddUpdateInfo(ref ValidationErrors validationErrors, Informations entity)
        {
            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    if (AddUpdateInfo(ref validationErrors, db, entity))
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
        /// 添加信息
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="db"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, SysEntities db, Informations entity)
        {
            using (var repository = new InformationsRepository())
            {
                try
                {
                    if (CheckTitle(entity))
                    {
                        validationErrors.Add("： 使用标题不能重复，请重新输入标题！");
                        return false;
                    }
                    //else if (entity.FeedbackTemplateId.GetIdSort().Count==0)
                    //{
                    //    validationErrors.Add("： 态度反馈模板不能为空！");
                    //    return false;
                    //}
                    else if (entity.MemberId.GetIdSort().Count == 0)
                    {
                        validationErrors.Add("： 会员不能为空！");
                        return false;
                    }
                    if (entity.FeedbackTemplateId.GetIdSort().Count > 4)
                    {
                        validationErrors.Add("： 态度反馈模板最多只能设定4个！");
                        return false;
                    }
                    else
                    {
                        repository.Create(entity);
                        AddInfoMemberAndFeedbackTemplate(db, entity);
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
        /// 添加信息
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, Informations entity)
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
        /// 删除信息
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(ref ValidationErrors validationErrors, string id)
        {
            using (var repository = new InformationsRepository())
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
        /// 删除信息集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="deleteCollection">主键的信息</param>
        /// <returns></returns>    
        public bool DeleteCollection(ref ValidationErrors validationErrors, string[] deleteCollection)
        {
            using (var repository = new InformationsRepository())
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
        /// 编辑一个信息
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个数据字典</param>
        /// <returns></returns>
        public bool Edit(ref ValidationErrors validationErrors, SysEntities db, Informations entity)
        {
            using (var repository = new InformationsRepository())
            {
                try
                {
                    if (CheckTitle(entity))
                    {
                        validationErrors.Add("： 使用标题不能重复，请重新输入标题！");
                        return false;
                    }
                    //else if (entity.FeedbackTemplateId.GetIdSort().Count == 0)
                    //{
                    //    validationErrors.Add("： 态度反馈模板不能为空！");
                    //    return false;
                    //}
                    else if (entity.MemberId.GetIdSort().Count == 0)
                    {
                        validationErrors.Add("： 会员不能为空！");
                        return false;
                    }
                    else if (entity.FeedbackTemplateId.GetIdSort().Count > 4)
                    {
                        validationErrors.Add("： 态度反馈模板最多只能设定4个！");
                        return false;
                    }
                    else
                    {
                        repository.Edit(db, entity);
                        repository.Save(db);
                        string deleteSql =
                                          @"
                                        DELETE FROM InformationFeedbackTemplate
                                        WHERE InformationId=@InformationId
                                        DELETE FROM InformationMember
                                        WHERE InformationId=@InformationId
                                        DELETE FROM MemberInformations
                                        WHERE InformationsId=@InformationId
                                        DELETE FROM InformationMerchant
                                        WHERE InformationId=@InformationId
                                        DELETE FROM MerchantInformations
                                        WHERE InformationsId=@InformationId
                                        ";
                        var deleteArg = new DbParameter[] { new SqlParameter { ParameterName = "InformationId", Value = entity.Id } };
                        db.ExecuteStoreCommand(deleteSql, deleteArg);
                        AddInfoMemberAndFeedbackTemplate(db, entity);
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
        /// 编辑一个信息
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个信息</param>
        /// <returns>是否编辑成功</returns>
        public bool Edit(ref ValidationErrors validationErrors, Informations entity)
        {
            using (var repository = new MemberGroupRepository())
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
        }

        /// <summary>
        /// 获取所有信息
        /// </summary>
        /// <returns></returns>
        public List<Informations> GetAll()
        {
            using (var repository = new InformationsRepository())
            {
                return repository.GetAll(db).ToList();
            }
        }

        /// <summary>
        /// 根据用户Id获取他其所发的信息
        /// </summary>
        /// <param name="personId">用户Id</param>
        /// <returns></returns>
        public List<Informations> GetInformationsBySysPersonId(string personId)
        {
            return db.Informations.Where(o => o.CreatePersonId == personId).ToList();
        }

        /// <summary>
        /// 根据主键获取一个信息
        /// </summary>
        /// <param name="id">信息的主键</param>
        /// <returns>一个商家信息</returns>
        public Informations GetById(string id)
        {
            using (var repository = new InformationsRepository())
            {
                return repository.GetById(db, id);
            }
        }

        /// <summary>
        /// 信息反馈统计
        /// </summary>
        /// <param name="informationId">信息编号</param>
        /// <returns></returns>
        public InformationFeedbackReport GetInformationFeedbackReport(string informationId)
        {
            InformationFeedbackReport report = new InformationFeedbackReport();
            var index1 = 0;
            var index2 = 0;
            using (var repository = new InformationsRepository())
            {
                try
                {
                    string sql = @"
                    SELECT  Title ,
                            CreateTime
                    FROM    dbo.Informations
                    WHERE   Id = @InformationId

                    SELECT  FT.Id AS FeedbackTemplateId ,
                            Name ,
                            ( SELECT    COUNT(Id) Amount
                              FROM      InformationFeedback IFB
                              WHERE     TYPE <> 1
                                        AND InformationsId = @InformationId
                                        AND IFB.FeedbackTemplateId = FT.Id
                            ) Amount
                    FROM    InformationFeedbackTemplate IFT
                            INNER JOIN FeedbackTemplate FT ON IFT.FeedbackTemplateId = FT.Id
                    WHERE   InformationId = @InformationId

                    SELECT  M.Name AS MemberName ,
                            M.Contacts ,
                            M.Phone ,
                            FT.Name AS FeedbackTemplateName ,
                            IFB.Question ,
                            CreateTime
                    FROM    InformationFeedback IFB
                            INNER JOIN dbo.Member M ON IFB.MemberId = M.Id
                            LEFT JOIN dbo.FeedbackTemplate FT ON IFB.FeedbackTemplateId = FT.Id
                    WHERE   InformationsId = @InformationId
                            AND IFB.Question IS NOT NULL

                    SELECT  M.Name AS MemberName ,
                            M.Contacts ,
                            M.Phone
                    FROM    dbo.InformationMember IM
                            INNER JOIN dbo.Member M ON IM.MemberId = M.Id
                    WHERE   IM.InformationId = @InformationId
                            AND IM.MemberId NOT IN ( SELECT MemberId
                                                     FROM   InformationFeedback
                                                     WHERE  InformationsId = @InformationId )

                    SELECT  COUNT(*) AS Total
                    FROM    InformationMember
                    WHERE   InformationId = @InformationId

                --信息商家统计
                    SELECT  FT.Id AS FeedbackTemplateId ,
                            Name ,
                            ( SELECT    COUNT(Id) Amount
                              FROM      InformationFeedback IFB
                              WHERE     TYPE <> 0
                                        AND InformationsId = @InformationId
                                        AND IFB.FeedbackTemplateId = FT.Id
                            ) Amount
                    FROM    InformationFeedbackTemplate IFT
                            INNER JOIN FeedbackTemplate FT ON IFT.FeedbackTemplateId = FT.Id
                    WHERE   InformationId = @InformationId

                    SELECT  M.Name AS MemberName ,
                            M.Contacts ,
                            M.Cellphone AS Phone ,
                            FT.Name AS FeedbackTemplateName ,
                            IFB.Question ,
                            CreateTime
                    FROM    InformationFeedback IFB
                            INNER JOIN dbo.Merchant M ON IFB.MemberId = M.Id
                            LEFT JOIN dbo.FeedbackTemplate FT ON IFB.FeedbackTemplateId = FT.Id
                    WHERE   InformationsId = @InformationId
                            AND IFB.Question IS NOT NULL

                    SELECT  M.Name AS MemberName ,
                            M.Contacts ,
                            M.Cellphone AS Phone
                    FROM    dbo.InformationMerchant IM
                            INNER JOIN dbo.Merchant M ON IM.MerchantId = M.Id
                    WHERE   IM.InformationId = @InformationId
                            AND IM.MerchantId NOT IN ( SELECT MemberId
                                                     FROM   InformationFeedback
                                                     WHERE  InformationsId = @InformationId )

                    SELECT  COUNT(*) AS Total
                    FROM    InformationMerchant
                    WHERE   InformationId = @InformationId";
                    SqlCommand scmd = new SqlCommand(sql);

                    scmd.Parameters.AddWithValue("@InformationId", informationId);
                    var dataSources = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd);
                    var dataSource1 = dataSources.Tables[0];
                    var dataSource2 = dataSources.Tables[1];
                    var dataSource3 = dataSources.Tables[2];
                    var dataSource4 = dataSources.Tables[3];
                    var dataSource5 = dataSources.Tables[4];
                    var dataSource6 = dataSources.Tables[5];
                    var dataSource7 = dataSources.Tables[6];
                    var dataSource8 = dataSources.Tables[7];
                    var dataSource9 = dataSources.Tables[8];
                    for (int i = 0; i < dataSource1.Rows.Count; i++)
                    {
                        report.Title = dataSource1.Rows[i]["Title"].ToString();
                        report.SendTime = Convert.ToDateTime(dataSource1.Rows[i]["CreateTime"]);
                    }
                    foreach (DataRow item in dataSource2.Rows)
                    {
                        var f = new FeedbackTemplateStatistics();
                        f.FeedbackTemplateId = item["FeedbackTemplateId"].ToString();
                        f.FeedbackTemplateName = item["Name"].ToString();
                        f.FeedbackTemplateFeedbackAmount = Convert.ToInt32(item["Amount"]);
                        report.FeedbackMemberAmount += f.FeedbackTemplateFeedbackAmount;
                        report.FeedbackTemplateStatisticsList.Add(f);
                    }

                    foreach (DataRow item in dataSource3.Rows)
                    {
                        var m = new FeedbackTemlateMembers();
                        m.Index = index1 + 1;
                        m.MemberName = item["MemberName"].ToString();
                        m.Contacts = item["Contacts"] == null ? "" : item["Contacts"].ToString();
                        m.Phone = item["Phone"] == null ? "" : item["Phone"].ToString();
                        m.FeedbackTemplateName = item["FeedbackTemplateName"] == null ? "" : item["FeedbackTemplateName"].ToString();
                        m.CreateTime = Convert.ToDateTime(item["CreateTime"]);
                        m.FeedbackContent = item["Question"].ToString();
                        report.FeedbackTemlateMembersList.Add(m);
                        index1++;
                    }

                    foreach (DataRow item in dataSource4.Rows)
                    {
                        var m = new FeedbackTemlateMembers();
                        m.Index = index2 + 1;
                        m.MemberName = item["MemberName"].ToString();
                        m.Contacts = item["Contacts"] == null ? "" : item["Contacts"].ToString();
                        m.Phone = item["Phone"] == null ? "" : item["Phone"].ToString();
                        report.UnFeedbackTemlateMembersList.Add(m);
                        index2++;
                    }

                    foreach (DataRow item in dataSource5.Rows)
                    {
                        report.ReceiveMemberAmount = Convert.ToInt32(item["Total"]);
                    }

                    foreach (DataRow item in dataSource6.Rows)
                    {
                        var f = new FeedbackTemplateStatistics();
                        f.FeedbackTemplateId = item["FeedbackTemplateId"].ToString();
                        f.FeedbackTemplateName = item["Name"].ToString();
                        f.FeedbackTemplateFeedbackAmount = Convert.ToInt32(item["Amount"]);
                        report.FeedbackMerchantAmount += f.FeedbackTemplateFeedbackAmount;
                        report.FeedbackTemplateMerchantStatisticsList.Add(f);
                    }

                    foreach (DataRow item in dataSource7.Rows)
                    {
                        var m = new FeedbackTemlateMembers();
                        m.Index = index1 + 1;
                        m.MemberName = item["MemberName"].ToString();
                        m.Contacts = item["Contacts"] == null ? "" : item["Contacts"].ToString();
                        m.Phone = item["Phone"] == null ? "" : item["Phone"].ToString();
                        m.FeedbackTemplateName = item["FeedbackTemplateName"] == null ? "" : item["FeedbackTemplateName"].ToString();
                        m.CreateTime = Convert.ToDateTime(item["CreateTime"]);
                        m.FeedbackContent = item["Question"].ToString();
                        report.FeedbackTemlateMerchantsList.Add(m);
                        index1++;
                    }

                    foreach (DataRow item in dataSource8.Rows)
                    {
                        var m = new FeedbackTemlateMembers();
                        m.Index = index2 + 1;
                        m.MemberName = item["MemberName"].ToString();
                        m.Contacts = item["Contacts"] == null ? "" : item["Contacts"].ToString();
                        m.Phone = item["Phone"] == null ? "" : item["Phone"].ToString();
                        report.UnFeedbackTemlateMerchantsList.Add(m);
                        index2++;
                    }

                    foreach (DataRow item in dataSource9.Rows)
                    {
                        report.ReceiveMerchantAmount = Convert.ToInt32(item["Total"]);
                    }
                    if (report != null)
                    {
                        return report;
                    }

                    return null;
                }
                catch (Exception)
                {
                    return null;
                }

            }
        }

        public void Dispose()
        {

        }
    }
}
