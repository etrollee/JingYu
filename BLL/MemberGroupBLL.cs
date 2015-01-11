using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Common;
using DAL;
using IBLL;

namespace BLL
{
    public class MemberGroupBLL : IMemberGroupBLL, IDisposable
    {
        /// <summary>
        /// 私有的数据访问上下文
        /// </summary>
        protected SysEntities db;

         public MemberGroupBLL()
            : this(new SysPersonBLL())
        {
        }

        ISysPersonBLL _iSysPersonBll;
        /// <summary>
        /// 构造函数，默认加载数据访问上下文
        /// </summary>
        public MemberGroupBLL(SysPersonBLL sysPersonBll)
        {
            _iSysPersonBll = sysPersonBll;
            db = new SysEntities();
        }

           /// <summary>
        /// 验证会员分组名称是否存在
        /// </summary>
        /// <param name="entity">分组实体</param>
        /// <returns></returns>
        public bool CheckName(MemberGroup entity)
        {
            return db.MemberGroup.Any(o => o.Name == entity.Name 
                && o.CreatePersonId == entity.CreatePersonId&&o.Id!=entity.Id);
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
        public List<MemberGroup> GetByParam(string sysPersonId, string id, int page, int rows, string order,
            string sort, string search, ref int total)
        {

            using (var repository = new MemberGroupRepository())
            {
                IQueryable<MemberGroup> queryData =null;
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
        /// 创建一个会员分组
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="db">数据库上下文</param>
        /// <param name="entity">一个会员分组</param>
        /// <returns></returns>
        public bool Create(ref ValidationErrors validationErrors, SysEntities db, MemberGroup entity)
        {
            using (var repository = new MemberGroupRepository())
            {
                try
                {
                    int count = 1;
                    if (CheckName(entity))
                    {
                        validationErrors.Add("分组名称已被占用，请换一个新的分组名称");
                        return false;
                    }
                    foreach (string item in entity.MemberId.GetIdSort())
                    {
                        Member sys = new Member { Id = item };
                        db.Member.Attach(sys);
                        entity.Member.Add(sys);
                        count++;
                    }
                    repository.Create(db, entity);
                    if (count == repository.Save(db))
                    {
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

        public bool Create(ref ValidationErrors validationErrors,  MemberGroup entity)
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

        public bool Delete(ref ValidationErrors validationErrors, string id)
        {
            using (var repository = new MemberGroupRepository())
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
        /// 删除会员分组集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="deleteCollection">主键的会员分组</param>
        /// <returns></returns>    
        public bool DeleteCollection(ref ValidationErrors validationErrors, string[] deleteCollection)
        {
            using (var repository = new MemberGroupRepository())
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
        ///  创建会员分组集合
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entitys">会员分组集合</param>
        /// <returns></returns>
        public bool EditCollection(ref ValidationErrors validationErrors, IQueryable<MemberGroup> entitys)
        {
            if (entitys != null)
            {
                try
                {
                    int flag = 0, count = entitys.Count();
                    if (count > 0)
                    {
                        using (TransactionScope transactionScope = new TransactionScope())
                        {
                            foreach (var entity in entitys)
                            {
                                if (Edit(ref validationErrors, db, entity))
                                {
                                    flag++;
                                }
                                else
                                {
                                    Transaction.Current.Rollback();
                                    return false;
                                }
                            }
                            if (count == flag)
                            {
                                transactionScope.Complete();
                                return true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    validationErrors.Add(ex.Message);
                    ExceptionsHander.WriteExceptions(ex);
                }
            }
            return false;
        }

        /// <summary>
        /// 编辑一个会员分组
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="db">数据上下文</param>
        /// <param name="entity">一个会员分组</param>
        /// <returns>是否编辑成功</returns>
        public bool Edit(ref ValidationErrors validationErrors, SysEntities db, MemberGroup entity)
        {
            using (var repository = new MemberGroupRepository())
            {
                if (entity == null)
                {
                    return false;
                }
                if (CheckName(entity))
                {
                    validationErrors.Add("分组名称已被占用，请换一个新的分组名称");
                    return false;
                }

                int count = 1;
                MemberGroup editEntity = repository.Edit(db, entity);

                List<string> addMemberId = new List<string>();
                List<string> deleteMemberId = new List<string>();
                DataOfDiffrent.GetDiffrent(entity.MemberId.GetIdSort(), entity.MemberIdOld.GetIdSort(),
                    ref addMemberId, ref deleteMemberId);
                if (addMemberId != null && addMemberId.Count() > 0)
                {
                    foreach (var item in addMemberId)
                    {
                        Member sys = new Member { Id = item };
                        db.Member.Attach(sys);
                        editEntity.Member.Add(sys);
                        count++;
                    }
                }
                if (deleteMemberId != null && deleteMemberId.Count() > 0)
                {
                    List<Member> listEntity = new List<Member>();
                    foreach (var item in deleteMemberId)
                    {
                        Member sys = new Member { Id = item };
                        listEntity.Add(sys);
                        db.Member.Attach(sys);
                    }
                    foreach (Member item in listEntity)
                    {
                        editEntity.Member.Remove(item);//查询数据库
                        count++;
                    }
                }

                if (count == repository.Save(db))
                {
                    return true;
                }
                else
                {
                    validationErrors.Add("编辑会员分组出错了");
                }
                return false;
            }
        }
        /// <summary>
        /// 编辑一个会员分组
        /// </summary>
        /// <param name="validationErrors">返回的错误信息</param>
        /// <param name="entity">一个会员分组</param>
        /// <returns>是否编辑成功</returns>
        public bool Edit(ref ValidationErrors validationErrors, MemberGroup entity)
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
        /// 获取所有会员分组
        /// </summary>
        /// <returns></returns>
        public List<MemberGroup> GetAll()
        {
            using (var repository = new MemberGroupRepository())
            {
                return repository.GetAll(db).ToList();
            }
        }

        /// <summary>
        /// 根据主键获取一个会员分组
        /// </summary>
        /// <param name="id">会员分组的主键</param>
        /// <returns>一个会员分组</returns>
        public MemberGroup GetById(string id)
        {
            using (var repository = new MemberGroupRepository())
            {
                return repository.GetById(db, id);
            }
        }

        public void Dispose()
        {

        }
    }
}
