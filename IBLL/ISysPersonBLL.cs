﻿using System;
using System.Collections.Generic;
using System.Linq;

using Common;
using DAL;
using System.ServiceModel;

namespace IBLL
{
    /// <summary>
    /// 人员 接口
    /// </summary>
    [ServiceContract(Namespace = "solution")]
    public interface ISysPersonBLL
    {
        /// <summary>
        /// 验证用户名是否存在
        /// </summary>
        /// <param name="name">用户名</param>
        /// <returns></returns>
        bool CheckName(string name);
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
        List<SysPerson> GetByParam(string id, int page, int rows, string order, string sort, string search, ref int total);
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        System.Collections.Generic.List<SysPerson> GetAll();

          /// <summary>
        /// 获取在该表一条数据中，出现的所有外键实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>外键实体集合</returns>
        List<SysRole> GetRefSysRole(string id);

        /// <summary>
        /// 获取在该表一条数据中，出现的所有外键实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>外键实体集合</returns>
        List<Merchant> GetRefMerchant(string id);

        /// <summary>
        /// 获取在该表中出现的所有外键实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<SysRole> GetRefSysRole();

        /// <summary>
        /// 根据主键，查看详细信息
        /// </summary>
        /// <param name="id">根据主键</param>
        /// <returns></returns>
        [OperationContract]
        SysPerson GetById(string id);
        /// <summary>
        /// 创建一个对象
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个对象</param>
        /// <returns></returns>
        [OperationContract]
        bool Create(ref Common.ValidationErrors validationErrors, DAL.SysPerson entity);
        /// <summary>
        ///  创建对象集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entitys">对象集合</param>
        /// <returns></returns>
        [OperationContract]
        bool CreateCollection(ref Common.ValidationErrors validationErrors, System.Linq.IQueryable<DAL.SysPerson> entitys);
        /// <summary>
        /// 删除一个对象
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="id">一条数据的主键</param>
        /// <returns></returns>  
        [OperationContract]
        bool Delete(ref Common.ValidationErrors validationErrors, string id);
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
        bool Edit(ref Common.ValidationErrors validationErrors, DAL.SysPerson entity);
        /// <summary>
        ///  创建对象集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entitys">对象集合</param>
        /// <returns></returns>
        [OperationContract]
        bool EditCollection(ref Common.ValidationErrors validationErrors, System.Linq.IQueryable<DAL.SysPerson> entitys);

    }
}
