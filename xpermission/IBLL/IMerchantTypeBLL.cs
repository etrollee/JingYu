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
    /// 商家类型接口
    /// </summary>
    [ServiceContract(Namespace = "solution")]
    public interface IMerchantTypeBLL
    {
        /// <summary>
        /// 验证商家类型是否存在
        /// </summary>
        /// <param name="name">商家类型</param>
        /// <returns></returns>
        bool CheckName(MerchantType entity);

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
        List<MerchantType> GetByParam(int id, int page, int rows, string order, string sort, string search, ref int total);

        /// <summary>
        /// 获取所有商家类别
        /// </summary>
        /// <returns></returns>
        List<MerchantType> GetAll();

        /// <summary>
        /// 根据主键，查看详细信息
        /// </summary>
        /// <param name="id">根据主键</param>
        /// <returns></returns>
        [OperationContract]
        MerchantType GetById(int id);

        /// <summary>
        /// 创建一个对象
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个对象</param>
        /// <returns></returns>
        [OperationContract]
        bool Create(ref ValidationErrors validationErrors,MerchantType entity);

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
        bool Edit(ref ValidationErrors validationErrors, MerchantType entity); 
    }
}
