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
    public class RegisterCodeBLL : IRegisterCodeBLL, IDisposable
    {
        /// <summary>
        /// 私有的数据访问上下文
        /// </summary>
        protected SysEntities db;

        public RegisterCodeBLL()
            : this(new SysPersonBLL())
        {
        }

        ISysPersonBLL _iSysPersonBll;
        /// <summary>
        /// 构造函数，默认加载数据访问上下文
        /// </summary>
        public RegisterCodeBLL(SysPersonBLL sysPersonBll)
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
        public List<RegisterCode> GetByParam(int id, int page, int rows, string order,
            string sort, string search, ref int total)
        {
            using (var repository = new RegisterCodeRepository())
            {
                IQueryable<RegisterCode> queryData = repository.DaoChuData(db, order, sort, search, null);
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
        public List<RegisterCode> GetMerchantRegisterCodes(string sysPersonId, string id, int page,
            int rows, string order, string sort,
            string search, ref int total)
        {
            using (var repository = new RegisterCodeRepository())
            {
                var sysRole = _iSysPersonBll.GetRefSysRole(sysPersonId).FirstOrDefault();
                var merchant = _iSysPersonBll.GetRefMerchant(sysPersonId).FirstOrDefault();
                string merchantId = string.Empty;
                if (sysRole.Power == 1 || sysRole.Power == 2)
                {
                    merchantId = null;
                }
                else
                {
                    merchantId = merchant.Id;
                }
                var query = repository.DaoChuData(db, order, sort, search, merchantId);
                total = query.Count();
                if (total > 0)
                {
                    if (page <= 1)
                    {
                        query = query.Take(rows);
                    }
                    else
                    {
                        query = query.Skip((page - 1) * rows).Take(rows);
                    }
                }
                return query.ToList();
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
        public List<RegisterCode> GetRegisterCodeLogins(int id, int page, int rows, string order,
            string sort, string search, ref int total)
        {
            using (var repository = new RegisterCodeRepository())
            {
                var query = repository.GetRegisterCodeLogin(db, order, sort, search);
                total = query.Count();
                if (total > 0)
                {
                    if (page <= 1)
                    {
                        query = query.Take(rows);
                    }
                    else
                    {
                        query = query.Skip((page - 1) * rows).Take(rows);
                    }
                }
                return query.ToList();
            }
        }

        /// <summary>
        /// 获取所有注册码
        /// </summary>
        /// <returns></returns>
        public List<RegisterCode> GetAll()
        {
            using (var repository = new RegisterCodeRepository())
            {
                return repository.GetAll(db).ToList();
            }
        }

        /// <summary>
        /// 根据主键，查看详细信息
        /// </summary>
        /// <param name="id">根据主键</param>
        /// <returns></returns>
        public RegisterCode GetById(int id)
        {
            using (var repository = new RegisterCodeRepository())
            {
                return repository.GetById(db, id);
            }
        }

        /// <summary>
        /// 获取count个随机码
        /// </summary>
        /// <param name="count">获取随机码数量</param>
        /// <returns></returns>
        public List<RegularCode> GetRegularCodes(int count)
        {
            return db.RegularCode.Where(o => o.IsUsed == false).Take(count).ToList();
        }

        /// <summary>
        /// 根据注册码获取对应的信息
        /// </summary>
        /// <param name="value">注册码</param>
        /// <returns></returns>
        public RegisterCode GetRegisterCodeByValue(string value)
        {
            using (var repository = new RegisterCodeRepository())
            {
                return repository.GetByValue(db, value);
            }
        }
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="arrNum"></param>
        /// <param name="temp"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="ra"></param>
        /// <returns></returns>
        private int getNum(int[] arrNum, int temp, int minValue, int maxValue, Random ra)
        {
            int n = 0;
            while (n <= arrNum.Length - 1)
            {
                if (arrNum[n] == temp)//利用循环判断是否有重复       
                {
                    temp = ra.Next(minValue, maxValue); //重新随机获取。          
                    getNum(arrNum, temp, minValue, maxValue, ra);//递归:如果取出来的数字和已取得的数字有重复就重新随机获取。      
                }
                n++;
            }
            return temp;
        }

        /// <summary>
        /// 生成随机数数组
        /// </summary>
        /// <param name="num"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        private int[] GetRandomNum(int num, int minValue, int maxValue)
        {
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int[] arrNum = new int[num];
            int temp = 0;
            for (int i = 0; i <= num - 1; i++)
            {
                temp = ra.Next(minValue, maxValue);//取随机数
                temp = ra.Next(minValue, maxValue); //随机取数      
                arrNum[i] = getNum(arrNum, temp, minValue, maxValue, ra); //取出值赋到数组中
            }
            return arrNum;
        }

        /// <summary>
        /// 批量生成注册码
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="count">生成注册码个数</param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, SysEntities db, RegisterCode entity)
        {
            using (var dataContext = new SysEntities())
            {
                try
                {
                    string temp = string.Empty;
                    var dbNum = GetRegularCodes(entity.Count);
                    int[] randomNums = GetRandomNum(entity.Count, 1000, 9999);
                    string updateSql = @"UPDATE RegularCode SET IsUsed=1 WHERE Id=@Id";
                    string insertSql = @"INSERT INTO RegisterCode(Value,CreateTime)VALUES(@Value,GetDate()) SELECT @@IDENTITY AS Id";

                    int i = 0;
                    foreach (var item in dbNum)
                    {
                        temp = randomNums[i].ToString() + item.Code;
                        List<char> oldList = new List<char>();
                        oldList.AddRange(temp.ToCharArray());
                        List<char> newList = new List<char>();
                        Random ra = new Random();
                        while (oldList.Count > 0)//打乱生成的字符
                        {
                            int nIndex = ra.Next(0, oldList.Count - 1);
                            newList.Add(oldList[nIndex]);
                            oldList.Remove(oldList[nIndex]);
                        }
                        SqlCommand scmdUpdate = new SqlCommand(updateSql);
                        var arg = new DbParameter[] { new SqlParameter { ParameterName = "Id", Value = item.Id } };
                        scmdUpdate.Parameters.AddRange(arg);
                        ObjectQueryExtensions.ExceuteEntityProcReturnDataset(dataContext, scmdUpdate);

                        SqlCommand scmd = new SqlCommand(insertSql);
                        scmd.Parameters.AddWithValue("@Value", newList.ToArray());
                        var dataSource = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(dataContext, scmd).Tables[0];
                        int registerCodeId = -1;
                        foreach (DataRow data in dataSource.Rows)
                        {
                            registerCodeId = Convert.ToInt32(data["Id"]);
                        }

                        if (entity.MerchantId != null)
                        {
                            string merchantRegisterCodeSql = @"
                            UPDATE RegisterCode SET IsDistribution=1,BelongMerchantId=@MerchantId WHERE Id=@RegisterCodeId";
                            SqlCommand scmdMerchantRegisterCode = new SqlCommand(merchantRegisterCodeSql);
                            var args = new DbParameter[] {
                            new SqlParameter { ParameterName = "RegisterCodeId", Value =registerCodeId },
                            new SqlParameter{ParameterName="MerchantId",Value=entity.MerchantId}
                        };
                            scmdMerchantRegisterCode.Parameters.AddRange(args);
                            ObjectQueryExtensions.ExceuteEntityProcReturnDataset(dataContext, scmdMerchantRegisterCode);
                        }

                        i++;
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
        /// 批量生成注册码
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="count">生成注册码个数</param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, RegisterCode entity)
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
        /// 编辑一个对象
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个对象</param>
        /// <returns></returns>
        public bool Edit(ref ValidationErrors validationErrors, RegisterCode entity)
        {
            using (var repository = new RegisterCodeRepository())
            {
                try
                {
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
        /// 删除一个对象
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="id">一条数据的主键</param>
        /// <returns></returns>  
        public bool Delete(ref ValidationErrors validationErrors, int id)
        {
            using (var repository = new RegisterCodeRepository())
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
        /// 删除对象集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="deleteCollection">主键的集合</param>
        /// <returns></returns>       
        public bool DeleteCollection(ref Common.ValidationErrors validationErrors, string[] deleteCollection)
        {
            using (var repository = new RegisterCodeRepository())
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
        /// 编辑商家注册码
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个对象</param>
        /// <returns></returns>
        public bool EditMerchantRegisterCode(ref ValidationErrors validationErrors, MerchantRegisterCode entity)
        {
            using (var dataContext = new SysEntities())
            {
                try
                {
                    string sql = @"UPDATE RegisterCode SET IsValid=@IsValid WHERE Id=@RegisterCodeId";
                    SqlCommand scmd = new SqlCommand(sql);
                    var args = new DbParameter[] { 
                        new SqlParameter { ParameterName = "IsValid",Value=entity.IsValid },
                        new SqlParameter { ParameterName = "RegisterCodeId",Value=entity.Id}
                    };
                    scmd.Parameters.AddRange(args);

                    return ObjectQueryExtensions.ExecuteNonQuery(dataContext, scmd) > 0;
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
        /// 删除一个对象
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="id">一条数据的主键</param>
        /// <returns></returns>  
        public bool DeleteMerchantRegisterCode(ref ValidationErrors validationErrors, int id)
        {
            using (var dataContext = new SysEntities())
            {
                try
                {
                    string sql = @"
                    SELECT  RC.Id ,
                            Value ,
                            IsUsed ,
                            IsDistribution ,
                            RC.IsValid ,
                            RC.BelongMerchantId,
                            RC.MerchantId,
                            RC.MemberId,
                            MT.Name AS BelongMerchant ,
                            M.Name AS MerchantName ,
                            Mem.Name AS MemberName
                    FROM    RegisterCode RC
                            LEFT JOIN Merchant M ON RC.MerchantId = M.Id
                            LEFT JOIN Merchant MT ON RC.BelongMerchantId = MT.Id
                            LEFT JOIN Member Mem ON RC.MemberId = Mem.Id
                            WHERE RC.Id=@RegisterCodeId";
                    SqlCommand scmd = new SqlCommand(sql);
                    scmd.Parameters.AddWithValue("@RegisterCodeId", id);

                    var dataSource = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(dataContext, scmd).Tables[0];
                    foreach (DataRow item in dataSource.Rows)
                    {
                        var merchantId = item["MerchantId"].ToString();
                        var memberId = item["MemberId"].ToString();
                        var merchantName = item["MerchantName"].ToString();
                        var memberName = item["MemberName"].ToString();
                        var name = merchantName == null ? memberName : merchantName;
                        if (!string.IsNullOrEmpty(merchantId) || !string.IsNullOrEmpty(memberId))
                        {
                            validationErrors.Add("该注册码正被" + name + "使用，不能删除！");
                            return false;
                        }
                    }

                    string deleteSql = @"
                            DELETE FROM RegisterCode WHERE Id=@RegisterCodeId";
                    SqlCommand scmdDelete = new SqlCommand(deleteSql);
                    scmdDelete.Parameters.AddWithValue("@RegisterCodeId", id);

                    return ObjectQueryExtensions.ExecuteNonQuery(dataContext, scmdDelete) > 0;
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MerchantRegisterCode GetMerchantRegisterCodeById(int id)
        {
            MerchantRegisterCode merchantRegisterCode = new MerchantRegisterCode();
            using (var dataContext = new SysEntities())
            {
                try
                {
                    string sql = @"
                        SELECT  MR.Id ,
                                R.Id AS RegisterCodeId ,
                                R.Value AS RegisterCode ,
                                R.IsValid ,
                                R.IsUsed
                        FROM    MerchantRegisterCode MR
                                INNER JOIN RegisterCode R ON MR.RegisterCodeId = R.Id
                        WHERE   MR.Id=@Id";
                    SqlCommand scmd = new SqlCommand(sql);
                    scmd.Parameters.AddWithValue("@Id", id);
                    var dataSource = ObjectQueryExtensions.ExceuteEntityProcReturnDataset(dataContext, scmd).Tables[0];
                    foreach (DataRow item in dataSource.Rows)
                    {
                        merchantRegisterCode.Id = Convert.ToInt32(item["Id"]);
                        merchantRegisterCode.RegisterCodeId = Convert.ToInt32(item["RegisterCodeId"]);
                        merchantRegisterCode.RegisterCode = item["RegisterCode"].ToString();
                        merchantRegisterCode.IsValid = Convert.ToBoolean(item["IsValid"]);
                        merchantRegisterCode.IsUsed = Convert.ToBoolean(item["IsUsed"]);
                    }
                    if (merchantRegisterCode != null)
                    {
                        return merchantRegisterCode;
                    }

                    return null;
                }
                catch (Exception)
                {
                    return null;
                    throw;
                }
            }
        }

        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="id">一条数据的主键</param>
        /// <returns></returns>  
        public bool DeleteRegisterCodeLogin(ref ValidationErrors validationErrors, int id)
        {
            using (var db = new SysEntities())
            {
                try
                {
                    string sql = @"DELETE FROM RegisterCodeLogin WHERE Id=@Id";
                    SqlCommand scmd = new SqlCommand(sql);
                    scmd.Parameters.AddWithValue("@Id", id);

                    return ObjectQueryExtensions.ExecuteNonQuery(db, scmd) > 0;
                }
                catch (Exception ex)
                {
                    validationErrors.Add(ex.Message);
                    ExceptionsHander.WriteExceptions(ex);
                    return false;
                }
            }
        }

        public void Dispose()
        {

        }
    }
}
