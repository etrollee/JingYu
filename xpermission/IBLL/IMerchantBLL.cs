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
    /// 商家接口
    /// </summary>
    [ServiceContract(Namespace = "solution")]
    public interface IMerchantBLL
    {
        /// <summary>
        /// 验证商家名称是否存在
        /// </summary>
        /// <param name="name">商家</param>
        /// <returns></returns>
        bool CheckName(Merchant entity);

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
        List<Merchant> GetByParam(string merchantId, string id, int page, int rows, string order, string sort, string search, ref int total);

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        List<Merchant> GetAll();

        /// <summary>
        /// 获取商家的Id、名称、注册码和上级商家Id
        /// </summary>
        /// <returns></returns>
        List<dynamic> GetAllMerchant();

        /// <summary>
        /// 获取一个商家类别下的商家
        /// </summary>
        /// <param name="merchantTypeId">商家类别Id</param>
        /// <returns></returns>
        List<Merchant> GetMerchantsByMerchantTypeId(int merchantTypeId);

        /// <summary>
        /// 根据主键，查看详细信息
        /// </summary>
        /// <param name="id">根据主键</param>
        /// <returns></returns>
        [OperationContract]
        Merchant GetById(string id);

        /// <summary>
        /// 获取在该表一条数据中，出现的所有外键实体
        /// </summary>
        /// <param name="id">外键</param>
        /// <returns>外键实体集合</returns>
        Merchant GetRefSysPerson(string id);

        /// <summary>
        /// 创建一个对象
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个对象</param>
        /// <returns></returns>
        [OperationContract]
        bool Create(ref ValidationErrors validationErrors, Merchant entity);


        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="id">一条数据的主键</param>
        /// <returns></returns>  
        [OperationContract]
        bool Delete(ref ValidationErrors validationErrors, string id);

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
        /// <param name="editAction">编辑模式(0：系统管理员或客服编辑，1：商家自己编辑)</param>
        /// <returns></returns>
        [OperationContract]
        bool Edit(ref ValidationErrors validationErrors, Merchant entity,int editAction);

        /// <summary>
        ///  编辑对象集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entitys">对象集合</param>
        /// <returns></returns>
        [OperationContract]
        bool EditCollection(ref ValidationErrors validationErrors, IQueryable<Merchant> entitys,int editAction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantId"></param>
        /// <param name="oldParentId"></param>
        /// <param name="newParentId"></param>
        /// <param name="oldChildIds"></param>
        /// <param name="newChildIds"></param>
        /// <param name="validationErrors"></param>
        /// <returns></returns>
        bool SetRelationShip(string merchantId, string oldParentId, string newParentId,
            string[] oldChildIds, string[] newChildIds, ref ValidationErrors validationErrors);

        /// <summary>
        /// 修改商家自己的信息
        /// </summary>
        /// <param name="validationErrors"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        bool ChangeSelfInfo(ref ValidationErrors validationErrors, Merchant entity);

        /// <summary>
        /// 获取商家Id
        /// </summary>
        /// <param name="syspersonId">用户Id</param>
        /// <returns></returns>
        string GetMerchantId(string syspersonId);
    }
}
