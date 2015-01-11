using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Common;
using DAL;
using IBLL;

namespace BLL
{
    public class ClientManagerBLL : IClientManagerBLL, IDisposable
    {
        Regex filterRegex = new Regex(@"<[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// 登录后修改注册已被使用
        /// </summary>
        /// <param name="registerCode">注册码</param>
        public void ChangeIsUsed(string registerCode)
        {
            using (var db = new SysEntities())
            {
                string sql = @"UPDATE RegisterCode SET IsUsed=1 WHERE Value=@RegisterCode";
                var arg = new DbParameter[] { new SqlParameter { ParameterName = "RegisterCode", Value = registerCode } };
                db.ExecuteStoreCommand(sql, arg);
            }
        }

        /// <summary>
        /// 获取会员没有阅读及没有反馈的信息数量
        /// </summary>
        /// <param name="memberType">会员：1；商家：2</param>
        /// <param name="memberId">会员Id</param>
        /// <returns></returns>
        public List<int> GetUnChangeMessageCount(int memberType,string memberId, int type, int update)
        {
            using (var db = new SysEntities())
            {
                List<int> list = new List<int>();
                string selectSql = string.Empty;
                if (memberType==1)
                {
                    selectSql = @"
                    SELECT  COUNT(MInfo.Id) AS total
                        FROM    dbo.MemberInformations MInfo
                        INNER JOIN dbo.Informations Info ON MInfo.InformationsId = Info.Id
                        WHERE   IsRead = 0 AND MemberId=@MemberId {0}

                    SELECT  COUNT(MInfo.Id) AS total
                        FROM    dbo.MemberInformations MInfo
                        WHERE   IsFeedback = 0 AND MemberId=@MemberId";
                }
                else
                {
                    selectSql = @"
                    SELECT  COUNT(MInfo.Id) AS total
                        FROM    dbo.MerchantInformations MInfo
                        INNER JOIN dbo.Informations Info ON MInfo.InformationsId = Info.Id
                        WHERE   IsRead = 0 AND MerchantId=@MemberId {0}

                    SELECT  COUNT(MInfo.Id) AS total
                        FROM    dbo.MerchantInformations MInfo
                        WHERE   IsFeedback = 0 AND MerchantId=@MemberId";
                }

                string sql = "";
                if (update == 0)
                {
                    sql = string.Concat(" AND Info.Type<>3");
                }
                if (type == 1)
                {
                    sql = string.Concat(" AND Info.Type=2");
                }
                if (type == 1 && update == 1)
                {
                    sql = string.Concat(" AND Info.Type<>1");
                }

                selectSql = string.Format(selectSql, sql);
                SqlCommand scmd = new SqlCommand(selectSql);
                scmd.Parameters.AddWithValue("@MemberId", memberId);
                var dataSourse0 = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd).Tables[0];

                foreach (DataRow item in dataSourse0.Rows)
                {
                    list.Add(Convert.ToInt32(item["total"]));
                }
                string selectSql1 = string.Empty;
                if (memberType==1)
                {
                    selectSql1 = @"
                    SELECT  COUNT(MInfo.Id) AS total
                        FROM    dbo.MemberInformations MInfo
                        WHERE   IsFeedback = 0 AND MemberId=@MemberId";
                }
                else
                {
                    selectSql1 = @"
                    SELECT  COUNT(MInfo.Id) AS total
                        FROM    dbo.MerchantInformations MInfo
                        WHERE   IsFeedback = 0 AND MerchantId=@MemberId";
                }

                SqlCommand scmd1 = new SqlCommand(selectSql1);
                scmd1.Parameters.AddWithValue("@MemberId", memberId);
                var dataSourse1 = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd1).Tables[0];

                foreach (DataRow item in dataSourse1.Rows)
                {
                    list.Add(Convert.ToInt32(item["total"]));
                }

                return list;
            }
        }

        /// <summary>
        /// 通过注册码获取用户信息
        /// </summary>
        /// <param name="registerCode">注册码</param>
        /// <returns></returns>
        public User GetUserByRegisterCode(string registerCode, string serialPort, out string message, out string welcomeInfo)
        {
            using (var db = new SysEntities())
            {
                try
                {
                    string info = string.Empty;
                    var obj = GetRegisterCode(registerCode);
                    if (obj == null)
                    {
                        message = "注册码不存在";
                        welcomeInfo = null;
                        return null;
                    }
                    else if (!obj.IsValid)
                    {
                        message = "注册码已被注销，请与客服或商家联系";
                        welcomeInfo = null;
                        return null;
                    }
                    User user = new User();
                    string sql = @"SELECT Id,Name,RegisterCode,IsValid,VIP FROM Member WHERE RegisterCode=@RegisterCode
                                   SELECT Id,Name,RegisterCode FROM Merchant WHERE RegisterCode=@RegisterCode";

                    SqlCommand scmd = new SqlCommand(sql);

                    scmd.Parameters.AddWithValue("@RegisterCode", registerCode);
                    var dataSources = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd);
                    var member = dataSources.Tables[0];
                    var merchant = dataSources.Tables[1];
                    var registerCodeLogin = GetRegisterCodeLogin(registerCode);
                    if (member.Rows.Count > 0)
                    {
                        foreach (DataRow item in member.Rows)
                        {
                            user.Id = item["Id"].ToString();
                            user.Name = item["Name"].ToString();
                            user.Code = item["RegisterCode"].ToString();
                            user.IsValid = Convert.ToBoolean(item["IsValid"].ToString());
                            user.Type = Convert.ToBoolean(item["VIP"]) ? 1 : 0;
                            user.MemberType = 1;
                        }
                        if (!user.IsValid)
                        {
                            message = "该会员已被注销，请与客服或商家联系";
                            welcomeInfo = null;
                            return null;
                        }
                        if (!obj.IsUsed)
                        {
                            ChangeIsUsed(registerCode);
                        }
                        if (registerCodeLogin == null)
                        {
                            RememberLogin(registerCode, serialPort);
                            info = GetSysParas().WelcomeInfo;
                        }
                        if (registerCodeLogin != null && serialPort != registerCodeLogin.SerialPort)
                        {
                            message = "一个注册码只能用于一台手机";
                            welcomeInfo = null;
                            return null;
                        }
                        UpdateRegisterCodeLogin(registerCode);
                        message = "登录成功";
                        welcomeInfo = info;
                        return user;
                    }
                    if (merchant.Rows.Count > 0)
                    {
                        foreach (DataRow item in merchant.Rows)
                        {
                            user.Id = item["Id"].ToString();
                            user.Name = item["Name"].ToString();
                            user.Code = item["RegisterCode"].ToString();
                            user.IsValid = false;
                            user.Type = 2;
                            user.MemberType = 2;
                        }
                        if (!obj.IsUsed)
                        {
                            ChangeIsUsed(registerCode);
                        }
                        if (registerCodeLogin == null)
                        {
                            RememberLogin(registerCode, serialPort);
                            info = GetSysParas().WelcomeInfo;
                        }
                        if (registerCodeLogin != null && serialPort != registerCodeLogin.SerialPort)
                        {
                            message = "一个注册码只能用于一台手机";
                            welcomeInfo = null;
                            return null;
                        }
                        UpdateRegisterCodeLogin(registerCode);
                        message = "登录成功";
                        welcomeInfo = info;
                        return user;
                    }
                }
                catch (Exception)
                {
                    message = "登录失败";
                    welcomeInfo = null;
                    return null;
                }
                message = "登录失败";
                welcomeInfo = null;
                return null;
            }
        }

        public RegisterCodeLogin GetRegisterCodeLogin(string registerCode)
        {
            using (var dataContext = new SysEntities())
            {
                return dataContext.RegisterCodeLogin.Where(o => o.RCode == registerCode).FirstOrDefault();
            }
        }


        /// <summary>
        /// 根据注册码获取注册码信息
        /// </summary>
        /// <param name="code">注册码</param>
        /// <returns></returns>
        private RegisterCode GetRegisterCode(string code)
        {
            using (var db = new SysEntities())
            {
                return db.RegisterCode.SingleOrDefault(o => o.Value == code);
            }
        }

        /// <summary>
        /// 记录注册码已登录
        /// </summary>
        /// <param name="registerCode">注册码</param>
        /// <returns></returns>
        public bool RememberLogin(string registerCode, string serialPort)
        {
            using (var db = new SysEntities())
            {
                var registerCodeLogin = new RegisterCodeLogin();
                registerCodeLogin.RCode = registerCode;
                registerCodeLogin.LoginTime = DateTime.Now;
                registerCodeLogin.SerialPort = serialPort;
                db.AddToRegisterCodeLogin(registerCodeLogin);
                if (db.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool UpdateRegisterCodeLogin(string registerCode)
        {
            using (var db = new SysEntities())
            {
                var obj = db.RegisterCodeLogin.Where(o => o.RCode == registerCode).FirstOrDefault();
                obj.LoginTime = DateTime.Now;
                return db.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 检测注册码是否已登录
        /// </summary>
        /// <param name="registerCode">注册码</param>
        /// <returns></returns>
        public RegisterCodeLogin CheckRegisterCodeIsLogined(string registerCode)
        {
            using (var db = new SysEntities())
            {
                var obj = db.RegisterCodeLogin.SingleOrDefault(o => o.LoginTime == null && o.RCode == registerCode);
                if (obj != null)
                {
                    return obj;
                }
                return null;
            }
        }

        /// <summary>
        /// 注册码退出登录
        /// </summary>
        /// <param name="registerCode">注册码</param>
        /// <returns></returns>
        public bool SignOut(string registerCode)
        {
            using (var db = new SysEntities())
            {
                var code = db.RegisterCodeLogin.SingleOrDefault(o => o.RCode == registerCode);
                if (code != null)
                {
                    code.LoginTime = null;
                    if (db.SaveChanges() > 0)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
        }

        /// <summary>
        /// 通过会员Id获取其前count条未读信息
        /// </summary>
        /// <param name="memberType">会员：1；商家：2</param>
        /// <param name="memberId">会员Id</param>
        /// <param name="count">前count条</param>
        /// <returns></returns>
        public List<Informations> GetInformations(int memberType,string memberId, int type, int update, int count)
        {
            List<Informations> informationList = new List<Informations>();
            using (SysEntities db = new SysEntities())
            {
                try
                {
                    string selectSql = string.Empty;
                    if (memberType==1)
                    {
                        selectSql = @"SELECT Info.Id,Title,Type,TimeLimit,Content,CreatePersonId,P.Name AS PersonName,
                                    Info.CreateTime,MemberId,IsRead FROM dbo.Informations Info
                                    INNER JOIN dbo.MemberInformations MInfo ON MInfo.InformationsId=Info.Id
                                    INNER JOIN dbo.SysPerson P ON Info.CreatePersonId = P.Id
                                    WHERE MInfo.MemberId=@MemberId {0}
                                    ORDER BY IsRead, Info.CreateTime DESC
                                SELECT Id,Name,InfoFT.InformationId  FROM dbo.FeedbackTemplate FT
                                INNER JOIN dbo.InformationFeedbackTemplate InfoFT
                                ON FT.Id = InfoFT.FeedbackTemplateId
                                WHERE InfoFT.InformationId IN(SELECT Info.Id FROM dbo.Informations Info
                                    INNER JOIN dbo.MemberInformations MInfo 
                                    ON MInfo.InformationsId=Info.Id
                                    WHERE MInfo.MemberId=@MemberId {0})";
                    }
                    else
                    {
                        selectSql = @"SELECT Info.Id,Title,Type,TimeLimit,Content,CreatePersonId,P.Name AS PersonName,
                                    Info.CreateTime,MerchantId AS MemberId,IsRead FROM dbo.Informations Info
                                    INNER JOIN dbo.MerchantInformations MInfo ON MInfo.InformationsId=Info.Id
                                    INNER JOIN dbo.SysPerson P ON Info.CreatePersonId = P.Id
                                    WHERE MInfo.MerchantId=@MemberId {0}
                                    ORDER BY IsRead, Info.CreateTime DESC
                                SELECT Id,Name,InfoFT.InformationId  FROM dbo.FeedbackTemplate FT
                                INNER JOIN dbo.InformationFeedbackTemplate InfoFT
                                ON FT.Id = InfoFT.FeedbackTemplateId
                                WHERE InfoFT.InformationId IN(SELECT Info.Id FROM dbo.Informations Info
                                    INNER JOIN dbo.MerchantInformations MInfo 
                                    ON MInfo.InformationsId=Info.Id
                                    WHERE MInfo.MerchantId=@MemberId {0})";
                    }

                    string sql = " AND 1=1";
                    if (update == 0)
                    {
                        sql = string.Concat(" AND Info.Type<>3");
                    }
                    if (type == 1)
                    {
                        sql = string.Concat(" AND Info.Type=2");
                    }
                    if (type == 1 && update == 1)
                    {
                        sql = string.Concat(" AND Info.Type<>1");
                    }

                    selectSql = string.Format(selectSql, sql);
                    SqlCommand scmd = new SqlCommand(selectSql);
                    scmd.Parameters.AddWithValue("@MemberId", memberId);
                    var dataSources = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd);
                    var infos = dataSources.Tables[0];
                    var feedbackTemplates = dataSources.Tables[1];
                    if (infos.Rows.Count > 0)
                    {
                        foreach (DataRow item in infos.Rows)
                        {
                            var info = new Informations();
                            info.Id = item["Id"].ToString();
                            info.Title = item["Title"].ToString();
                            info.Type = Convert.ToInt32(item["Type"]);
                            info.Content = filterRegex.Replace(item["Content"].ToString(), "");
                            info.TimeLimit = Convert.ToInt32(item["TimeLimit"]);
                            info.PersonName = item["PersonName"].ToString();
                            info.CreateTime = Convert.ToDateTime(item["CreateTime"]);
                            info.MemberId = item["MemberId"].ToString();
                            info.IsRead = Convert.ToBoolean(item["IsRead"]);

                            foreach (DataRow f in feedbackTemplates.Rows)
                            {
                                if (f["InformationId"].ToString() == item["Id"].ToString())
                                {
                                    var template = new FeedbackTemplate();
                                    template.Id = f["Id"].ToString();
                                    template.Name = f["Name"].ToString();
                                    info.FeedbackTemplates.Add(template);
                                }

                            }

                            informationList.Add(info);
                        }

                        return informationList.Take(count).ToList();
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 通过会员Id获取其所有的信息
        /// </summary>
        /// <param name="memberType">会员：1；商家：2</param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public List<Informations> GetInformations(int memberType,string memberId, int type, 
            int update, int pageIndex, int pageSize)
        {
            List<Informations> informationList = new List<Informations>();
            int startRow = (pageIndex - 1) * pageSize;
            using (SysEntities db = new SysEntities())
            {
                try
                {
                    string selectSql = string.Empty;
                    if (memberType==1)
                    {
                        selectSql = @"SELECT Info.Id,Title,Type,TimeLimit,Content,CreatePersonId,P.Name AS PersonName,
                                    Info.CreateTime,MemberId,IsRead FROM dbo.Informations Info
                                    INNER JOIN dbo.MemberInformations MInfo ON MInfo.InformationsId=Info.Id
                                    INNER JOIN dbo.SysPerson P ON Info.CreatePersonId = P.Id
                                    WHERE MInfo.MemberId=@MemberId {0}
                                    ORDER BY IsRead, Info.CreateTime DESC
                                SELECT Id,Name,InfoFT.InformationId FROM dbo.FeedbackTemplate FT
                                INNER JOIN dbo.InformationFeedbackTemplate InfoFT
                                ON FT.Id = InfoFT.FeedbackTemplateId
                                WHERE InfoFT.InformationId IN(SELECT Info.Id FROM dbo.Informations Info
                                    INNER JOIN dbo.MemberInformations MInfo 
                                    ON MInfo.InformationsId=Info.Id
                                    WHERE MInfo.MemberId=@MemberId {0}) ";
                    }
                    else
                    {
                        selectSql = @"SELECT Info.Id,Title,Type,TimeLimit,Content,CreatePersonId,P.Name AS PersonName,
                                    Info.CreateTime,MerchantId AS MemberId,IsRead FROM dbo.Informations Info
                                    INNER JOIN dbo.MerchantInformations MInfo ON MInfo.InformationsId=Info.Id
                                    INNER JOIN dbo.SysPerson P ON Info.CreatePersonId = P.Id
                                    WHERE MInfo.MerchantId=@MemberId {0}
                                    ORDER BY IsRead, Info.CreateTime DESC
                                SELECT Id,Name,InfoFT.InformationId FROM dbo.FeedbackTemplate FT
                                INNER JOIN dbo.InformationFeedbackTemplate InfoFT
                                ON FT.Id = InfoFT.FeedbackTemplateId
                                WHERE InfoFT.InformationId IN(SELECT Info.Id FROM dbo.Informations Info
                                    INNER JOIN dbo.MerchantInformations MInfo 
                                    ON MInfo.InformationsId=Info.Id
                                    WHERE MInfo.MerchantId=@MemberId {0}) ";
                    }


                    string sql = " AND 1=1";
                    if (update == 0)
                    {
                        sql = string.Concat(" AND Info.Type<>3");
                    }
                    if (type == 1)
                    {
                        sql = string.Concat(" AND Info.Type=2");
                    }
                    if (type == 1 && update == 1)
                    {
                        sql = string.Concat(" AND Info.Type<>1");
                    }

                    selectSql = string.Format(selectSql, sql);

                    SqlCommand scmd = new SqlCommand(selectSql);
                    scmd.Parameters.AddWithValue("@MemberId", memberId);
                    var dataSources = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd);
                    var infos = dataSources.Tables[0];
                    var feedbackTemplates = dataSources.Tables[1];
                    if (infos.Rows.Count > 0)
                    {
                        foreach (DataRow item in infos.Rows)
                        {
                            var info = new Informations();
                            info.Id = item["Id"].ToString();
                            info.Title = item["Title"].ToString();
                            info.Type = Convert.ToInt32(item["Type"]);
                            info.Content = filterRegex.Replace(item["Content"].ToString(), "");
                            info.TimeLimit = Convert.ToDouble(item["TimeLimit"]);
                            info.PersonName = item["PersonName"].ToString();
                            info.CreateTime = Convert.ToDateTime(item["CreateTime"]);
                            info.MemberId = item["MemberId"].ToString();
                            info.IsRead = Convert.ToBoolean(item["IsRead"]);


                            foreach (DataRow f in feedbackTemplates.Rows)
                            {
                                if (f["InformationId"].ToString() == item["Id"].ToString())
                                {
                                    var template = new FeedbackTemplate();
                                    template.Id = f["Id"].ToString();
                                    template.Name = f["Name"].ToString();
                                    info.FeedbackTemplates.Add(template);
                                }

                            }
                            informationList.Add(info);
                        }
                        return informationList.Skip(startRow).Take(pageSize).ToList();
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 通过商家Id获取其所发送的信息
        /// </summary>
        /// <param name="merchantId">商家Id</param>
        /// <returns></returns>
        public List<Informations> GetformationsByMerchantId(string merchantId, int pageIndex, int pageSize)
        {
            List<Informations> informationList = new List<Informations>();
            string createPersonId = string.Empty;
            int startRow = (pageIndex - 1) * pageSize;
            using (var db = new SysEntities())
            {
                try
                {
                    string sqlQueryPerson = @"SELECT SysPersonId,MerchantId FROM dbo.MerchantSysPerson
                            WHERE MerchantId=@MerchantId";
                    SqlCommand scmdQuery = new SqlCommand(sqlQueryPerson);
                    scmdQuery.Parameters.AddWithValue("@MerchantId", merchantId);
                    var person = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmdQuery).Tables[0];
                    if (person.Rows.Count > 0)
                    {
                        foreach (DataRow item in person.Rows)
                        {
                            createPersonId = item["SysPersonId"].ToString();
                        }
                    }

                    if (createPersonId != null)
                    {
                        string sql = @"SELECT Info.Id,Title,Type,TimeLimit,Content,CreatePersonId,P.Name AS PersonName,
                                    Info.CreateTime FROM dbo.Informations Info
                                    INNER JOIN dbo.SysPerson P ON Info.CreatePersonId = P.Id
                                    WHERE Info.CreatePersonId=@CreatePersonId 
                                    ORDER BY Info.CreateTime DESC";
                        SqlCommand scmd = new SqlCommand(sql);
                        scmd.Parameters.AddWithValue("@CreatePersonId", createPersonId);
                        var datas = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd).Tables[0];
                        if (datas.Rows.Count > 0)
                        {
                            foreach (DataRow item in datas.Rows)
                            {
                                var info = new Informations();
                                info.Id = item["Id"].ToString();
                                info.Title = item["Title"].ToString();
                                info.Type = Convert.ToInt32(item["Type"]);
                                info.Content = filterRegex.Replace(item["Content"].ToString(), "");
                                info.TimeLimit = Convert.ToDouble(item["TimeLimit"]);
                                info.PersonName = item["PersonName"].ToString();
                                info.CreateTime = Convert.ToDateTime(item["CreateTime"]);
                                informationList.Add(info);
                            }
                            return informationList.Skip(startRow).Take(pageSize).ToList();
                        }
                    }

                    return informationList;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 通过Id获取信息
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="Id">信息id</param>
        /// <param name="memberId">会员Id</param>
        /// <returns></returns>
        public Informations GetInformationById(int type,string Id, string memberId)
        {
            Informations information = new Informations();
            using (SysEntities db = new SysEntities())
            {
                string sql = string.Empty;
                if (type==1)
                {
                     sql = @"SELECT Info.Id,Title,Type,Content,TimeLimit,
                                P.Name AS PersonName,Info.CreateTime,IsRead
                                FROM dbo.Informations Info
                                INNER JOIN dbo.SysPerson P ON Info.CreatePersonId = P.Id
                                INNER JOIN dbo.MemberInformations MInfo ON MInfo.InformationsId=Info.Id
                                WHERE  MInfo.InformationsId=@InformationId
                                AND MInfo.MemberId=@MemberId
                                SELECT Id,Name FROM dbo.FeedbackTemplate FT
                                LEFT JOIN dbo.InformationFeedbackTemplate InfoFT
                                ON FT.Id = InfoFT.FeedbackTemplateId
                                WHERE InfoFT.InformationId=@InformationId
                                SELECT M.Name AS MemberName,InfoFb.FeedbackTemplateId,
                                FT.Name AS    FeedbackTemplate,InfoFb.CreateTime,Question
                                FROM dbo.InformationFeedback InfoFb
                                    INNER JOIN dbo.Member M ON InfoFb.MemberId = M.Id
                                    INNER JOIN dbo.FeedbackTemplate FT ON InfoFb.FeedbackTemplateId=FT.Id
                                WHERE InformationsId=@InformationId AND MemberId=@MemberId";
                }
                else
                {
                     sql = @"SELECT Info.Id,Title,Type,Content,TimeLimit,
                                P.Name AS PersonName,Info.CreateTime,IsRead
                                FROM dbo.Informations Info
                                INNER JOIN dbo.SysPerson P ON Info.CreatePersonId = P.Id
                                INNER JOIN dbo.MerchantInformations MInfo ON MInfo.InformationsId=Info.Id
                                WHERE  MInfo.InformationsId=@InformationId
                                AND MInfo.MerchantId=@MemberId
                                SELECT Id,Name FROM dbo.FeedbackTemplate FT
                                LEFT JOIN dbo.InformationFeedbackTemplate InfoFT
                                ON FT.Id = InfoFT.FeedbackTemplateId
                                WHERE InfoFT.InformationId=@InformationId
                                SELECT M.Name AS MemberName,InfoFb.FeedbackTemplateId,
                                FT.Name AS    FeedbackTemplate,InfoFb.CreateTime,Question
                                FROM dbo.InformationFeedback InfoFb
                                    INNER JOIN dbo.Merchant M ON InfoFb.MemberId = M.Id
                                    INNER JOIN dbo.FeedbackTemplate FT ON InfoFb.FeedbackTemplateId=FT.Id
                                WHERE InformationsId=@InformationId AND MemberId=@MemberId";
                }

                SqlCommand scmd = new SqlCommand(sql);
                scmd.Parameters.AddWithValue("@InformationId", Id);
                scmd.Parameters.AddWithValue("@MemberId", memberId);
                var dataSources = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd);
                var info = dataSources.Tables[0];
                var feedbackTemplates = dataSources.Tables[1];
                var memberFeedback = dataSources.Tables[2];
                if (info.Rows.Count > 0)
                {
                    foreach (DataRow item in info.Rows)
                    {
                        information.Id = item["Id"].ToString();
                        information.Title = item["Title"].ToString();
                        information.Type = Convert.ToInt32(item["Type"]);
                        information.Content = filterRegex.Replace(item["Content"].ToString(), "");
                        information.TimeLimit = Convert.ToDouble(item["TimeLimit"]);
                        information.PersonName = item["PersonName"].ToString();
                        information.CreateTime = Convert.ToDateTime(item["CreateTime"]);
                        information.IsRead = Convert.ToBoolean(item["IsRead"]);
                    }
                    if (feedbackTemplates.Rows.Count > 0)
                    {
                        foreach (DataRow item in feedbackTemplates.Rows)
                        {
                            var template = new FeedbackTemplate();
                            template.Id = item["Id"].ToString();
                            template.Name = item["Name"].ToString();
                            information.FeedbackTemplates.Add(template);
                        }
                    }
                    if (memberFeedback.Rows.Count > 0)
                    {
                        foreach (DataRow item in memberFeedback.Rows)
                        {
                            information.InformationFeedbacks.FeedbackTemplateId = item["FeedbackTemplateId"].ToString();
                            information.InformationFeedbacks.FeedbackTemplateName = item["FeedbackTemplate"].ToString();
                            information.InformationFeedbacks.MemberName = item["MemberName"].ToString();
                            information.InformationFeedbacks.CreateTime = Convert.ToDateTime(item["CreateTime"]);
                            information.InformationFeedbacks.Question = item["Question"].ToString();
                        }
                    }
                    else
                    {
                        information.InformationFeedbacks.FeedbackTemplateId = "";
                        information.InformationFeedbacks.FeedbackTemplateName = "";
                        information.InformationFeedbacks.MemberName = "";
                        information.InformationFeedbacks.Question = "";
                    }
                }
                if (information != null)
                {
                    return information;
                }
                return null;
            }
        }

        /// <summary>
        /// 修改会员信息阅读状态
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="memberId">会员Id</param>
        /// <param name="informationId">信息Id</param>
        /// <returns></returns>
        public bool ChangeIsRead(int type,string memberId, string informationId)
        {
            using (var db = new SysEntities())
            {
                try
                {
                    string sql = string.Empty;
                    if (type==1)
                    {
                        sql = @"UPDATE MemberInformations SET IsRead=1 WhERE 
                        MemberId=@MemberId AND InformationsId=@InformationId";
                    }
                    else
                    {
                        sql = @"UPDATE MerchantInformations SET IsRead=1 WhERE 
                        MerchantId=@MemberId AND InformationsId=@InformationId";
                    }

                    var args = new DbParameter[] 
                        {
                            new SqlParameter{ParameterName="MemberId",Value=memberId},
                            new SqlParameter{ParameterName="InformationId",Value=informationId}
                        };
                    int result = db.ExecuteStoreCommand(sql, args);
                    if (result > 0)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {

                    return false;
                }

            }
            return false;
        }

        /// <summary>
        /// 删除会员的某条信息记录
        /// </summary
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="memberId">会员Id</param>
        /// <param name="informationId">信息Id</param>
        /// <returns></returns>
        public bool DeleteInformation(int type,string memberId, string informationId)
        {
            using (SysEntities db = new SysEntities())
            {
                try
                {
                    string sql = string.Empty;
                    if (type==1)
                    {
                        sql = @"DELETE FROM InformationMember WHERE MemberId=@MemberId AND InformationId=@InformationId
                                  DELETE FROM MemberInformations WHERE MemberId=@MemberId AND InformationsId=@InformationId";
                    }
                    else
                    {
                        sql = @"DELETE FROM InformationMerchant WHERE MerchantId=@MemberId AND InformationId=@InformationId
                                  DELETE FROM MerchantInformations WHERE MerchantId=@MemberId AND InformationsId=@InformationId";
                    }
                    var param = new DbParameter[]
                    { 
                        new SqlParameter { ParameterName = "MemberId",Value=memberId },
                        new SqlParameter { ParameterName = "InformationId",Value=informationId }
                    };
                    int result = db.ExecuteStoreCommand(sql, param);
                    if (result > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 删除会员的所有信息记录
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="memberId">会员Id</param>
        /// <returns></returns>
        public bool DeleteInformations(int type ,string memberId)
        {
            using (SysEntities db = new SysEntities())
            {
                try
                {
                    string sql = string.Empty;
                    if (type==1)
                    {
                        sql = @"DELETE FROM InformationMember WHERE MemberId=@MemberId 
                                  DELETE FROM MemberInformations WHERE MemberId=@MemberId";
                    }
                    else
                    {
                        sql = @"DELETE FROM InformationMerchant WHERE MerchantId=@MemberId 
                                  DELETE FROM MerchantInformations WHERE MerchantId=@MemberId";
                    }

                    var param = new DbParameter[]
                    { 
                        new SqlParameter { ParameterName = "MemberId",Value=memberId } 
                    };
                    int result = db.ExecuteStoreCommand(sql, param);
                    if (result > 0)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取商家类型
        /// </summary>
        /// <param name="count">前count个,为null时表示获取所有</param>
        /// <returns></returns>
        public List<MerchantType> GetMerchantTypes(int? pageIndex, int? pageSize)
        {
            List<MerchantType> merchantTypeList = null;
            var index = pageIndex ?? 1;
            var size = pageSize ?? int.MaxValue;

            int startRow = (index - 1) * size;
            using (SysEntities db = new SysEntities())
            {
                if (pageIndex == null || pageSize == null)
                {
                    merchantTypeList = db.MerchantType.ToList();
                }
                else
                {
                    merchantTypeList = db.MerchantType.OrderByDescending(t => t.Id).Skip(startRow).Take(size).ToList();
                }
                return merchantTypeList;
            }
        }

        /// <summary>
        /// 获取所有商家
        /// </summary>
        /// <param name="count">前count个,为null时表示获取所有</param>
        /// <returns></returns>
        public List<Merchant> GetMerchants(int? count, int pageIndex, int pageSize)
        {
            List<Merchant> merchantList = null;
            int startRow = (pageIndex - 1) * pageSize;
            using (SysEntities db = new SysEntities())
            {
                if (count != null)
                {
                    merchantList = db.Merchant.Take(count.Value).Skip(startRow).Take(pageSize).ToList();
                }
                else
                {
                    merchantList = db.Merchant.Skip(startRow).Take(pageSize).ToList();
                }

                return merchantList;
            }
        }

        /// <summary>
        /// 获取重点推荐商家
        /// </summary>
        /// <param name="count">前count个,为null时表示获取所有</param>
        /// <returns></returns>
        public List<Merchant> GetRecommendMerchants(int? count)
        {
            List<Merchant> merchantList = null;
            using (SysEntities db = new SysEntities())
            {
                if (count != null)
                {
                    merchantList = db.Merchant.OrderByDescending(o => o.ComprehensiveStar).Take(count.Value).ToList();
                }
                else
                {
                    merchantList = db.Merchant.OrderByDescending(o => o.ComprehensiveStar).ToList();
                }

                return merchantList;
            }
        }

        /// <summary>
        /// 通过商家类型Id获取其对应所有的商家
        /// </summary>
        /// <param name="merchantTypeId">商家类型Id</param>
        /// <returns></returns>
        public List<Merchant> GetMerchantsByMerchantTypeId(int merchantTypeId, int pageIndex, int pageSize)
        {
            int startRow = (pageIndex - 1) * pageSize; ;
            List<Merchant> merchantList = new List<Merchant>();
            using (var db = new SysEntities())
            {
                if (merchantTypeId > 0)
                {
                    merchantList = db.Merchant.Where(o => o.MerchantTypeId == merchantTypeId)
                   .OrderByDescending(o => o.ComprehensiveStar).Skip(startRow).Take(pageSize).ToList();
                }
                if (merchantTypeId == -1)
                {
                    merchantList = db.Merchant
                   .OrderByDescending(o => o.ComprehensiveStar).Skip(startRow).Take(pageSize).ToList();
                }
                return merchantList;
            }
        }

        /// <summary>
        /// 通过商家Id获取其详细信息
        /// </summary>
        /// <param name="Id">商家Id</param>
        /// <returns></returns>
        public Merchant GetMerchantById(string Id)
        {
            using (var db = new SysEntities())
            {
                return db.Merchant.SingleOrDefault(o => o.Id == Id);
            }
        }

        /// <summary>
        /// 通过商家Id获取其所有服务产品
        /// </summary>
        /// <param name="merchantId">商家Id</param>
        /// <returns></returns>
        public List<ServiceProduct> GetServiceProductsByMerchantId(string merchantId, int pageIndex, int pageSize)
        {
            int startRow = (pageIndex - 1) * pageSize;
            using (var db = new SysEntities())
            {
                return db.ServiceProduct.Where(o => o.MerchantId == merchantId)
                    .OrderByDescending(o => o.Star).Skip(startRow).Take(pageSize).ToList();
            }
        }

        /// <summary>
        /// 添加预约信息
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="appointment">预约实体</param>
        /// <returns></returns>
        public int AddAppointment(int type,Appointment appointment)
        {
            using (var db = new SysEntities())
            {
                var merchantProduct = GetMerchantProduct(appointment.ServiceProductId);
                if (merchantProduct.Worth > merchantProduct.Balance)
                {
                    return -1;
                }
                else
                {
                    var merchantId = db.ServiceProduct.SingleOrDefault(o => o.Id == appointment.ServiceProductId).MerchantId;
                    var informationId = Result.GetNewId();
                    var createPersonId = GetMerchantPerson(merchantId).SysPersonId;
                    string memberName = string.Empty;
                    string memberPhone = string.Empty;
                    if (type==1)
                    {
                        var member = db.Member.SingleOrDefault(o => o.Id == appointment.MemberId);
                        memberName = member.Name;
                        memberPhone = member.Phone;
                        appointment.MemberName = member.Name;
                    }
                    else
                    {
                        var merchant = db.Merchant.SingleOrDefault(o => o.Id == appointment.MemberId);
                        memberName = merchant.Name;
                        memberPhone = merchant.Cellphone;
                        appointment.MemberName = merchant.Name;
                    }

                    var memberInfo = new MemberInformations
                    {
                        InformationsId = informationId,
                        MemberId = merchantId,
                        IsRead = false,
                        IsFeedback = false
                    };
                    var info = new Informations
                    {
                        Id = informationId,
                        Title = "预约信息",
                        Content = "会员: " + memberName + "(联系方式'" + memberPhone + "') 预约了【"
                        + merchantProduct.ProductName + "】服务产品。",
                        TimeLimit = 2,
                        CreateTime = DateTime.Now,
                        CreatePersonId = createPersonId,
                        Type = 1
                    };
                    var balance = Convert.ToDecimal(merchantProduct.Balance - merchantProduct.Worth);
                    db.AddToAppointment(appointment);
                    db.AddToInformations(info);
                    db.AddToMemberInformations(memberInfo);
                    UpdateMerchant(merchantId, balance);
                    return db.SaveChanges() > 0 ? 1 : 0;
                }

            }
        }

        /// <summary>
        /// 请求商家联系方式
        /// </summary>
        /// <param name="id">商家Id</param>
        /// <returns></returns>
        public int RequestContact(string id)
        {
            using (var db = new SysEntities())
            {
                var merchant = db.Merchant.SingleOrDefault(o => o.Id == id);
                if (merchant != null)
                {
                    var deductMoney = GetSysParas().DeductMoney;
                    if (deductMoney > merchant.Balance)
                    {
                        return -1;
                    }
                    else
                    {
                        var balance = Convert.ToDecimal(merchant.Balance - deductMoney);
                        UpdateMerchant(id, balance);
                        return 1;
                    }
                }
                return 0;
            }
        }

        /// <summary>
        /// 获取商家服务产品
        /// </summary>
        /// <param name="productId">产品Id</param>
        /// <returns></returns>
        private dynamic GetMerchantProduct(int productId)
        {
            var merchantProduct = new List<dynamic>();
            using (var dataContext = new SysEntities())
            {
                string sql = @"
                    SELECT  M.Id AS MerchantId ,
                            M.Name AS MerchantName ,
                            M.Balance ,
                            SP.Name AS ProductName ,
                            SP.Worth
                    FROM    ServiceProduct SP
                            INNER JOIN Merchant M ON SP.MerchantId = M.Id
                    WHERE   SP.Id = @ProductId";
                SqlCommand scmd = new SqlCommand(sql);
                scmd.Parameters.AddWithValue("@ProductId", productId);
                var dataSource = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(dataContext, scmd).Tables[0];
                foreach (DataRow item in dataSource.Rows)
                {
                    var model = new
                    {
                        MerchantId = item["MerchantId"].ToString(),
                        MerchantName = item["MerchantName"].ToString(),
                        Balance = Convert.ToDecimal(item["Balance"]),
                        ProductName = item["ProductName"].ToString(),
                        Worth = Convert.ToDecimal(item["Worth"])
                    };
                    merchantProduct.Add(model);
                }

                return merchantProduct.FirstOrDefault();
            }
        }

        /// <summary>
        /// 获取商家用户信息
        /// </summary>
        /// <param name="merchantId">商家Id</param>
        /// <returns></returns>
        private dynamic GetMerchantPerson(string merchantId)
        {
            var merchantPerson = new List<dynamic>();
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
                foreach (DataRow item in dataSource.Rows)
                {
                    var model = new
                    {
                        MerchantId = item["MerchantId"].ToString(),
                        SysPersonId = item["SysPersonId"].ToString()
                    };
                    merchantPerson.Add(model);
                }

                return merchantPerson.FirstOrDefault();
            }
        }

        /// <summary>
        /// 更新商家余额
        /// </summary>
        /// <param name="merchantId">商家Id</param>
        /// <param name="balance">余额</param>
        private void UpdateMerchant(string merchantId, decimal balance)
        {
            using (var dataContext = new SysEntities())
            {
                string sql = @"UPDATE Merchant SET Balance=@Balance WHERE Id=@MerchantId";
                SqlCommand scmd = new SqlCommand(sql);
                scmd.Parameters.AddWithValue("@Balance", balance);
                scmd.Parameters.AddWithValue("@MerchantId", merchantId);

                ObjectQueryExtensions.ExceuteEntityProcReturnDataset(dataContext, scmd);
            }
        }

        /// <summary>
        /// 添加会员信息反馈
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="informationFeedback">信息反馈实体</param>
        /// <returns></returns>
        public bool AddInformationFeedback(int type,InformationFeedback informationFeedback)
        {
            int informationFeedbackId = -1;
            bool result = false;
            try
            {

                using (var db = new SysEntities())
                {
                    string selectSql = @"
                            SELECT  Id ,
                                    InformationsId ,
                                    MemberId
                            FROM    dbo.InformationFeedback
                            WHERE   InformationsId = @InformationId
                                    AND MemberId = @MemberId";

                    SqlCommand scmdSelect = new SqlCommand(selectSql);
                    scmdSelect.Parameters.AddWithValue("@InformationId", informationFeedback.InformationsId);
                    scmdSelect.Parameters.AddWithValue("@MemberId", informationFeedback.MemberId);
                    var dataSource = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmdSelect).Tables[0];
                    foreach (DataRow item in dataSource.Rows)
                    {
                        informationFeedbackId = Convert.ToInt32(item["Id"]);
                    }
                    if (informationFeedbackId > 0)
                    {
                        string updateSql = @"
                            UPDATE dbo.InformationFeedback
                            SET  FeedbackTemplateId=@FeedbackTemplateId,
                            CreateTime=@CreateTime,
                            Question=@Question
                            WHERE Id=@Id";

                        var arg = new DbParameter[] 
                                { 

                                     new SqlParameter{ParameterName="FeedbackTemplateId",
                                         Value=informationFeedback.FeedbackTemplateId},
                                     new SqlParameter{ParameterName="CreateTime",
                                         Value=informationFeedback.CreateTime},
                                     new SqlParameter{ParameterName="Question",
                                         Value=informationFeedback.Question},
                                         new SqlParameter{ParameterName="Id",
                                         Value=informationFeedbackId}
                                };
                        db.ExecuteStoreCommand(updateSql, arg);
                        result = true;
                    }
                    else
                    {
                        var memberName = GetMemberName(type, informationFeedback.MemberId);
                        string updateSql = string.Empty;
                        string sql = @"
                            INSERT   INTO InformationFeedback
                                    ( InformationsId ,
                                      FeedbackTemplateId ,
                                      MemberId ,
                                      MemberName,
                                      CreateTime ,
                                      Question,
                                      Type
                                    )
                           VALUES   ( @InformationsId ,
                                      @FeedbackTemplateId ,
                                      @MemberId ,
                                      @MemberName,  
                                      @CreateTime,
                                      @Question,
                                      @Type
                                    )";
                        var typeValue =type==1?0:1;
                        var args = new DbParameter[] 
                                { 
                                     new SqlParameter { ParameterName = "InformationsId",
                                         Value =informationFeedback.InformationsId},
                                     new SqlParameter{ParameterName="MemberId",Value=informationFeedback.MemberId},
                                     new SqlParameter{ParameterName="MemberName",Value=memberName},
                                     new SqlParameter{ParameterName="FeedbackTemplateId",
                                         Value=informationFeedback.FeedbackTemplateId},
                                     new SqlParameter{ParameterName="CreateTime",
                                         Value=informationFeedback.CreateTime},
                                     new SqlParameter{ParameterName="Question",
                                         Value=informationFeedback.Question},
                                     new SqlParameter{ParameterName="Type",
                                         Value=typeValue}
                                };
                        db.ExecuteStoreCommand(sql, args);

                        if (type==1)
                        {
                            updateSql = @"
                            UPDATE  MemberInformations
                            SET     IsFeedback = 1
                            WHERE   MemberId = @MemberId
                                    AND InformationsId = @InformationsId";
                        }
                        else
                        {
                            updateSql = @"
                            UPDATE  MerchantInformations
                            SET     IsFeedback = 1
                            WHERE   MerchantId = @MemberId
                                    AND InformationsId = @InformationsId";
                        }
                        var updateArgs= new DbParameter[] 
                                { 
                                     new SqlParameter { ParameterName = "InformationsId",
                                         Value =informationFeedback.InformationsId},
                                     new SqlParameter{ParameterName="MemberId",Value=informationFeedback.MemberId}
                                };
                        db.ExecuteStoreCommand(updateSql, updateArgs);


                        result = true;
                    }

                    return result;
                }
            }
            catch (Exception)
            {
                return result;
                throw;
            }

        }

        /// <summary>
        /// 获取会员名称
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private string GetMemberName(int type, string memberId)
        {
            string memberName = string.Empty;
            string sql = string.Empty;
            using (var db=new SysEntities())
            {
                if (type == 1)
                {
                    memberName = db.Member.SingleOrDefault(o => o.Id == memberId).Name;
                }
                else
                {
                    memberName = db.Merchant.SingleOrDefault(o => o.Id == memberId).Name;
                }
            }


            return memberName;
        }

        /// <summary>
        /// 信息反馈统计
        /// </summary>
        /// <param name="informationId">信息编号</param>
        /// <returns></returns>
        public InformationFeedbackReport GetInformationFeedbackReport(string informationId)
        {
            InformationFeedbackReport report = new InformationFeedbackReport();

            using (var db = new SysEntities())
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
                              WHERE     TYPE <>1
                                        AND InformationsId = @InformationId
                                        AND IFB.FeedbackTemplateId = FT.Id
                            ) Amount
                    FROM    InformationFeedbackTemplate IFT
                            INNER JOIN FeedbackTemplate FT ON IFT.FeedbackTemplateId = FT.Id
                    WHERE   InformationId = @InformationId

                    SELECT  M.Id ,
                            M.Name ,
                            IFB.Question ,
                            CreateTime
                    FROM    InformationFeedback IFB
                            INNER JOIN dbo.Member M ON IFB.MemberId = M.Id
                    WHERE   InformationsId = @InformationId
                            AND IFB.Question IS NOT NULL

                    SELECT  COUNT(*) AS Total
                    FROM    InformationMember
                    WHERE   InformationId = @InformationId";
                    SqlCommand scmd = new SqlCommand(sql);

                    scmd.Parameters.AddWithValue("@InformationId", informationId);
                    var dataSources = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(db, scmd);
                    var dataSource1 = dataSources.Tables[0];
                    var dataSource2 = dataSources.Tables[1];
                    var dataSource3 = dataSources.Tables[2];
                    var dataSource4 = dataSources.Tables[3];
                    for (int i = 0; i < dataSource1.Rows.Count; i++)
                    {
                        report.InformationId = informationId;
                        report.Title = dataSource1.Rows[i]["Title"].ToString();
                        report.SendTime = Convert.ToDateTime(dataSource1.Rows[i]["CreateTime"]);
                    }
                    foreach (DataRow item in dataSource2.Rows)
                    {
                        var f = new FeedbackTemplateStatistics();
                        f.FeedbackTemplateId = item["FeedbackTemplateId"].ToString();
                        f.FeedbackTemplateName = item["Name"].ToString();
                        f.FeedbackTemplateFeedbackAmount = Convert.ToInt32(item["amount"]);
                        report.FeedbackMemberAmount += f.FeedbackTemplateFeedbackAmount;
                        report.FeedbackTemplateStatisticsList.Add(f);
                    }

                    foreach (DataRow item in dataSource3.Rows)
                    {
                        var m = new FeedbackTemlateMembers();
                        m.MemberId = item["Id"].ToString();
                        m.MemberName = item["Name"].ToString();
                        m.CreateTime = Convert.ToDateTime(item["CreateTime"]);
                        m.FeedbackContent = item["Question"].ToString();
                        report.FeedbackTemlateMembersList.Add(m);
                    }

                    foreach (DataRow item in dataSource4.Rows)
                    {
                        report.ReceiveMemberAmount = Convert.ToInt32(item["Total"]);
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

        /// <summary>
        /// 获取系统配置信息
        /// </summary>
        /// <returns></returns>
        public SysParas GetSysParas()
        {
            using (var dataContext = new SysEntities())
            {
                return dataContext.SysParas.FirstOrDefault();
            }
        }

        public void Dispose()
        {

        }
    }
}
