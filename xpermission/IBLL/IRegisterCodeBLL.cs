using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Common;
using DAL;

namespace IBLL
{
    /// <summary>
    /// 注册码接口
    /// </summary>
    [ServiceContract(Namespace = "solution")]
    public interface IRegisterCodeBLL
    {
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
        [OperationContract]
        List<RegisterCode> GetByParam(int id, int page, int rows, string order, string sort, string search, ref int total);

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
        [OperationContract]
        List<RegisterCode> GetRegisterCodeLogins(int id, int page, int rows, string order,
            string sort, string search, ref int total);

        /// <summary>
        /// 获取所有注册码
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<RegisterCode> GetAll();

        /// <summary>
        /// 根据注册码获取对应的信息
        /// </summary>
        /// <param name="value">注册码</param>
        /// <returns></returns>
        [OperationContract]
        RegisterCode GetRegisterCodeByValue(string value);

        /// <summary>
        /// 根据主键，查看详细信息
        /// </summary>
        /// <param name="id">根据主键</param>
        /// <returns></returns>
        [OperationContract]
        RegisterCode GetById(int id);

        /// <summary>
        /// 获取count个随机码
        /// </summary>
        /// <param name="count">获取随机码数量</param>
        /// <returns></returns>
        List<RegularCode> GetRegularCodes(int count);

        /// <summary>
        /// 批量生成注册码
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="count">生成注册码个数</param>
        /// <returns></returns>
        [OperationContract]
        bool Create(ref ValidationErrors validationErrors, RegisterCode entity);

        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="id">一条数据的主键</param>
        /// <returns></returns>  
        [OperationContract]
        bool Delete(ref ValidationErrors validationErrors, int id);

        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="deleteCollection">主键的集合</param>
        /// <returns></returns>       
        [OperationContract]
        bool DeleteCollection(ref Common.ValidationErrors validationErrors, string[] deleteCollection);

        /// <summary>
        /// 编辑一个对象
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个对象</param>
        /// <returns></returns>
        [OperationContract]
        bool Edit(ref ValidationErrors validationErrors, RegisterCode entity);

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
        [OperationContract]
        List<RegisterCode> GetMerchantRegisterCodes(string sysPersonId, string id, 
            int page, int rows, string order, string sort,
            string search, ref int total);

        /// <summary>
        /// 编辑商家注册码
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个对象</param>
        /// <returns></returns>
        [OperationContract]
        bool EditMerchantRegisterCode(ref ValidationErrors validationErrors, MerchantRegisterCode entity);

        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="id">一条数据的主键</param>
        /// <returns></returns>  
        [OperationContract]
        bool DeleteMerchantRegisterCode(ref ValidationErrors validationErrors, int id);

        [OperationContract]
        MerchantRegisterCode GetMerchantRegisterCodeById(int id);

        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="id">一条数据的主键</param>
        /// <returns></returns>  
        [OperationContract]
        bool DeleteRegisterCodeLogin(ref ValidationErrors validationErrors, int id);
    }
}
