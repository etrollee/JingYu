using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using DAL;

namespace IBLL
{
    public interface IClientManagerBLL
    {
        /// <summary>
        /// 通过注册码获取用户信息
        /// </summary>
        /// <param name="registerCode">注册码</param>
        /// <returns></returns>
        User GetUserByRegisterCode(string registerCode, string serialPort, out string message, out string welcomeInfo);

        /// <summary>
        /// 检测注册码是否已登录
        /// </summary>
        /// <param name="registerCode">注册码</param>
        /// <returns></returns>
        RegisterCodeLogin CheckRegisterCodeIsLogined(string registerCode);

        /// <summary>
        /// 记录注册码已登录
        /// </summary>
        /// <param name="registerCode">注册码</param>
        /// <returns></returns>
        bool RememberLogin(string registerCode, string serialPort);

        /// <summary>
        /// 注册码退出登录
        /// </summary>
        /// <param name="registerCode">注册码</param>
        /// <returns></returns>
        bool SignOut(string registerCode);

        /// <summary>
        /// 通过会员Id获取其前count条未读信息
        /// </summary>
        /// <param name="memberType">会员：1；商家：2</param>
        /// <param name="memberId">会员Id</param>
        /// <param name="count">前count条</param>
        /// <returns></returns>
        List<Informations> GetInformations(int memberType,string memberId, int type, int update, int count);

        /// <summary>
        /// 通过会员Id获取其所有的信息
        /// </summary>
        /// <param name="memberType">会员：1；商家：2</param>
        /// <param name="memberId">会员Id</param>
        /// <returns></returns>
        List<Informations> GetInformations(int memberType,string memberId,int type,int update,int pageIndex,int pageSize);

        /// <summary>
        /// 通过Id获取信息
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="Id">信息id</param>
        /// <param name="memberId">会员Id</param>
        /// <returns></returns>
        Informations GetInformationById(int type,string Id,string memberId);

        /// <summary>
        /// 修改会员信息阅读状态
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="memberId">会员Id</param>
        /// <param name="informationId">信息Id</param>
        /// <returns></returns>
        bool ChangeIsRead(int type,string memberId,string informationId);

        /// <summary>
        /// 获取会员没有阅读及没有反馈的信息数量
        /// </summary>
        /// <param name="memberType">会员：1；商家：2</param>
        /// <param name="memberId">会员Id</param>
        /// <returns></returns>
        List<int> GetUnChangeMessageCount(int memberType,string memberId,int type, int update);

        /// <summary>
        /// 删除会员的某条信息记录
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="memberId">会员Id</param>
        /// <param name="informationId">信息Id</param>
        /// <returns></returns>
        bool DeleteInformation(int type,string memberId,string informationId);

        /// <summary>
        /// 删除会员的所有信息记录
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="memberId">会员Id</param>
        /// <returns></returns>
        bool DeleteInformations(int type,string memberId);

        /// <summary>
        /// 获取商家类型
        /// </summary>
        /// <param name="count">前count个,为null时表示获取所有</param>
        /// <returns></returns>
        List<MerchantType> GetMerchantTypes(int? pageIndex, int? pageSize);

        /// <summary>
        /// 获取所有商家
        /// </summary>
        /// <param name="count">前count个,为null时表示获取所有</param>
        /// <returns></returns>
        List<Merchant> GetMerchants(int? count, int pageIndex, int pageSize);

        /// <summary>
        /// 通过商家Id获取其所发送的信息
        /// </summary>
        /// <param name="merchantId">商家Id</param>
        /// <returns></returns>
        List<Informations> GetformationsByMerchantId(string merchantId,int pageIndex,int pageSize);

        /// <summary>
        /// 获取重点推荐商家
        /// </summary>
        /// <param name="count">前count个,为null时表示获取所有</param>
        /// <returns></returns>
        List<Merchant> GetRecommendMerchants(int? count);

        /// <summary>
        /// 通过商家类型Id获取其对应所有的商家
        /// </summary>
        /// <param name="merchantTypeId">商家类型Id</param>
        /// <returns></returns>
        List<Merchant> GetMerchantsByMerchantTypeId(int merchantTypeId, int pageIndex, int pageSize);

        /// <summary>
        /// 通过商家Id获取其详细信息
        /// </summary>
        /// <param name="Id">商家Id</param>
        /// <returns></returns>
        Merchant GetMerchantById(string Id);

        /// <summary>
        /// 请求商家联系方式
        /// </summary>
        /// <param name="id">商家Id</param>
        /// <returns></returns>
        int RequestContact(string id);

        /// <summary>
        /// 通过商家Id获取其所有服务产品
        /// </summary>
        /// <param name="merchantId">商家Id</param>
        /// <returns></returns>
        List<ServiceProduct> GetServiceProductsByMerchantId(string merchantId, int pageIndex, int pageSize);

        /// <summary>
        /// 添加预约信息
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="appointment">预约实体</param>
        /// <returns></returns>
        int AddAppointment(int type,Appointment appointment);

        /// <summary>
        /// 添加会员信息反馈
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="informationFeedback">信息反馈实体</param>
        /// <returns></returns>
        bool AddInformationFeedback(int type,InformationFeedback informationFeedback);

        /// <summary>
        /// 信息反馈统计
        /// </summary>
        /// <param name="informationId">信息编号</param>
        /// <returns></returns>
        InformationFeedbackReport GetInformationFeedbackReport(string informationId);
    }
}
