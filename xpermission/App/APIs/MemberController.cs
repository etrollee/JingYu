using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using BLL;
using DAL;
using IBLL;
using Newtonsoft.Json;

namespace App.APIs
{
    public class MemberController : ApiController
    {
        IClientManagerBLL _iClientManagerBLL;
        IInformationsBLL _iInformationsBLL;
        IMerchantBLL _iMerchantBll;

        public MemberController()
            : this(new ClientManagerBLL(), new InformationsBLL(), new MerchantBLL())
        {

        }

        public MemberController(ClientManagerBLL clientManagerBLL, InformationsBLL informationBll,
            MerchantBLL merchantBll)
        {
            _iClientManagerBLL = clientManagerBLL;
            _iInformationsBLL = informationBll;
            _iMerchantBll = merchantBll;
        }

        /// <summary>
        /// 删除会员某条信息
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="memberId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Description("删除会员某条信息")]
        public HttpResponseMessage DeleteMessage(int type,string memberId, string id)
        {
            try
            {
                if (string.IsNullOrEmpty(memberId) || string.IsNullOrEmpty(id))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { success = false, msg = "会员Id和信息Id均不能为空" });
                }
                var result = _iClientManagerBLL.DeleteInformation(type,memberId, id);
                if (result)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "删除成功" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "删除失败" });
                }

            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "删除时出现异常" });
            }

        }

        /// <summary>
        /// 删除会员所有的信息
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Description("删除会员所有的信息")]
        public HttpResponseMessage ClearMessage(int type,string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { success = false, msg = "会员Id不能为空" });
                }
                var result = _iClientManagerBLL.DeleteInformations(type,id);
                if (result)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "删除成功" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "删除失败" });
                }

            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "删除时出现异常" });
            }
        }

        /// <summary>
        /// 预约
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="memberId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Description("预约")]
        public HttpResponseMessage Appointment(int type,string memberId, int id)
        {
            try
            {
                if (string.IsNullOrEmpty(memberId) || string.IsNullOrEmpty(id.ToString()))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { success = false, msg = "会员Id和产品Id均不能为空", overInfo = "" });
                }
                var merchant = _iMerchantBll.GetById(memberId);
                //if (merchant != null)
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK,
                //        new { success = false, msg = "商家不参与预约业务", overInfo = "" });
                //}
                var appointment = new Appointment
                {
                    MemberId = memberId,
                    ServiceProductId = id,
                    CreateTime = DateTime.Now
                };
                var result = _iClientManagerBLL.AddAppointment(type,appointment);
                if (result == -1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new
                        { 
                            success = false, 
                            msg = "预约失败", 
                            overInfo = "该商家账户余额不足，请选择其他服务商或致电0771-2508531" 
                        });
                }
                else if (result == 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { success = true, msg = "预约成功", overInfo = "" });
                }
                else if (result == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { success = false, msg = "预约失败", overInfo = "" });
                }

            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                    new
                    {
                        success = false, 
                        msg = "发送预约失败",
                        overInfo = "" 
                    });
            }
            return Request.CreateResponse(HttpStatusCode.OK,
                new 
                { 
                    success = false, 
                    msg = "发送预约失败", 
                    overInfo = "" 
                });
        }

        [HttpGet]
        [Description("请求联系方式")]
        public HttpResponseMessage RequestContact(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id.ToString()))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { success = false, msg = "商家Id均不能为空", overInfo = "" });
                }

                var result = _iClientManagerBLL.RequestContact(id);
                if (result == -1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new 
                        {
                            success = false,
                            msg = "请求失败", 
                            overInfo = "该商家账户余额不足，请选择其他服务商或致电0771-2508531"
                        });
                }
                else if (result == 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { success = true, msg = "请求成功", overInfo = "" });
                }
                else if (result == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { success = false, msg = "请求失败", overInfo = "" });
                }

            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "请求失败", overInfo = "" });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "请求失败", overInfo = "" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="memberId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Description("修改信息阅读状态")]
        public HttpResponseMessage ChangeIsRead(int type,string memberId, string id)
        {
            try
            {
                if (string.IsNullOrEmpty(memberId) || string.IsNullOrEmpty(id))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { success = false, msg = "会员Id和信息Id均不能为空" });
                }
                var result = _iClientManagerBLL.ChangeIsRead(type,memberId, id);
                if (result)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "状态修改成功" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "状态修改失败" });
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "状态修改失败" });
            }
        }

        /// <summary>
        /// 信息反馈
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="memberId"></param>
        /// <param name="messageId"></param>
        /// <param name="templateId"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        [HttpGet]
        [Description("信息反馈")]
        public HttpResponseMessage MessageFeedback(int type,string memberId, string messageId, string templateId, string question)
        {
            try
            {
                if (string.IsNullOrEmpty(memberId) || string.IsNullOrEmpty(messageId) || string.IsNullOrEmpty(templateId))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { success = false, msg = "会员Id、信息Id、反馈模板Id均不能为空", overInfo = "" });
                }
                var info = _iInformationsBLL.GetById(messageId);
                TimeSpan ts = DateTime.Now - info.CreateTime;
                if (ts.TotalHours > info.TimeLimit)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { success = false, msg = "信息反馈失败", overInfo = "反馈超时" });
                }
                var infoFeedback = new InformationFeedback
                {
                    MemberId = memberId,
                    InformationsId = messageId,
                    FeedbackTemplateId = templateId,
                    Question = question,
                    CreateTime = DateTime.Now
                };

                var result = _iClientManagerBLL.AddInformationFeedback(type,infoFeedback);
                if (result)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "信息反馈成功", overInfo = "" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { success = false, msg = "信息反馈失败", overInfo = "" });
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "信息反馈失败", overInfo = "" });
            }
        }

        /// <summary>
        /// 获取会员所有的信息
        /// </summary>
        /// <param name="memberType">会员：1；商家：2</param>
        /// <param name="memberId"></param>
        /// <param name="type"></param>
        /// <param name="update"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Description("获取会员所有的信息")]
        public HttpResponseMessage Messages(int memberType, string memberId, int type, int update, string pageIndex, string pageSize)
        {
            try
            {
                if (string.IsNullOrEmpty(memberId))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { success = false, msg = "会员Id不能为空" });
                }
                if (string.IsNullOrWhiteSpace(pageIndex))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = false,
                        msg = "页码不能为空"
                    });
                }
                var intRegex = new Regex(@"^\d+$", RegexOptions.Compiled | RegexOptions.Singleline);
                if (!intRegex.IsMatch(pageIndex))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = false,
                        msg = "页码必须是整数"
                    });
                }
                var index = Convert.ToInt32(pageIndex);
                if (index < 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = false,
                        msg = "页码不能小于1"
                    });
                }

                if (string.IsNullOrWhiteSpace(pageSize))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = false,
                        msg = "每页记录数不能为空"
                    });
                }
                if (!intRegex.IsMatch(pageSize))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = false,
                        msg = "每页记录数必须是整数"
                    });
                }
                var size = Convert.ToInt32(pageSize);
                if (size < 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = false,
                        msg = "每页记录数不能小于1"
                    });
                }

                var messages = _iClientManagerBLL.GetInformations(memberType,memberId, type, update, index, size);

                if (messages == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new { success = false, msg = "找不到信息" });
                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = true,
                        msg = "请求成功",
                        data = messages.Select(m => new
                        {
                            Id = m.Id,
                            Title = m.Title,
                            Type = m.Type,
                            PublishTime = m.CreateTime.ToString("yyyy:MM:dd HH:mm:ss"),
                            IsRead = m.IsRead
                        })
                    });
                }

            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "获取信息失败" });
            }
        }

        /// <summary>
        /// 获取会员没有阅读及没有反馈的信息数量
        /// </summary>
        /// <param name="memberType">会员：1；商家：2</param>
        /// <param name="memberId"></param>
        /// <param name="type"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpGet]
        [Description("获取会员没有阅读及没有反馈的信息数量")]
        public HttpResponseMessage GetUnChangeMesssageCount(int memberType,string memberId, int type, int update)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(memberId))
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                       new { success = false, msg = "会员Id不能为空" });
                }
                else
                {
                    var list = _iClientManagerBLL.GetUnChangeMessageCount(memberType,memberId, type, update);

                    return Request.CreateResponse(HttpStatusCode.OK,
                        new
                        {
                            success = true,
                            data = new
                            {
                                UnReadCount = list[0],
                                UnFeedbackCount = list[1]
                            }
                        });
                }
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "获取信息失败" });
            }
        }
    }
}
