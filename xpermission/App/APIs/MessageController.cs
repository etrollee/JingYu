using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using BLL;
using DAL;
using IBLL;

namespace App.APIs
{
    public class MessageController : ApiController
    {
        IClientManagerBLL _iClientManagerBLL;
        public MessageController()
            : this(new ClientManagerBLL())
        {

        }

        public MessageController(ClientManagerBLL clientManagerBLL)
        {
            _iClientManagerBLL = clientManagerBLL;
        }

        /// <summary>
        /// 获取某条信息明细
        /// </summary>
        /// <param name="type">会员：1；商家：2</param>
        /// <param name="id"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpGet]
        [Description("获取某条信息明细")]
        public HttpResponseMessage Detail(int type,string id,string memberId)
        {
            try
            {
                if (string.IsNullOrEmpty(id)||string.IsNullOrWhiteSpace(memberId))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, 
                        new 
                        { 
                            success = false,
                            msg = "信息Id和会员Id均不能为空" 
                        });
                }
                var msgs = _iClientManagerBLL.GetInformationById(type,id,memberId);
                if (msgs == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, 
                        new 
                        { 
                            success = false,
                            msg = "信息不存在" 
                        });
                }

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    msg = "请求成功",
                    data = new
                    {
                        Id = msgs.Id,
                        Title = msgs.Title,
                        Type=msgs.Type,
                        Content = msgs.Content,
                        PublishTime = msgs.CreateTime.ToString("yyyy:MM:dd HH:mm:ss"),
                        PublishPerson = msgs.PersonName,
                        TimeLimit = msgs.TimeLimit,
                        IsRead = msgs.IsRead,
                        FeedbackTemplates = from f in msgs.FeedbackTemplates
                                            select new
                                            {
                                                Id = f.Id,
                                                Name = f.Name
                                            },
                        MemberInformationFeedback =new
                        {
                            FeedbackTemplateId = 
                            msgs.InformationFeedbacks.FeedbackTemplateId ,
                            Question = msgs.InformationFeedbacks.Question
                        }
                    }
                });
            }
            catch (Exception)
            {

                return Request.CreateResponse(HttpStatusCode.OK, 
                    new 
                    { 
                        success = false,
                        msg = "请求失败"
                    });
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberType">会员：1；商家：2</param>
        /// <param name="memberId"></param>
        /// <param name="type"></param>
        /// <param name="update"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet]
        [Description("获取会员前count条信息")]
        public HttpResponseMessage GetTopMessage(int memberType,string memberId, int type, int update, string count)
        {
            try
            {
                if (string.IsNullOrEmpty(memberId))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, 
                        new
                        {
                            success = false,
                            msg = "会员Id不能为空"
                        });
                }
                var isInt = new Regex(@"^\d+$", RegexOptions.Compiled | RegexOptions.Singleline).IsMatch(count);
                if (!isInt)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                      {
                          success = false,
                          msg = " 信息条数必须是整数"
                      });
                }
                var amount = Convert.ToInt32(count.Trim());
                if (amount < 1)
                {

                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = false,
                        msg = "信息条数必须大于0"
                    });
                }

                var msgs = _iClientManagerBLL.GetInformations(memberType,memberId,type, update, amount);
                if (msgs==null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, 
                        new 
                        { 
                            success = false,
                            msg = "信息不存在" 
                        });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK,
                        new
                        {
                            success = true,
                            msg = "请求成功",
                            data = msgs.Select(m => new
                            {
                                Id = m.Id,
                                Title = m.Title,
                                Type=m.Type,
                                PublishTime = m.CreateTime.ToString("yyyy:MM:dd HH:mm:ss"),
                                IsRead=m.IsRead
                            })});
                            }

            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.OK,
                    new 
                    { 
                        success = false, 
                        msg = "请求出现异常" 
                    });
            }
        }

    }
}
