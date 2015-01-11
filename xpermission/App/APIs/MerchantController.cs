using BLL;
using DAL;
using IBLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace App.APIs
{
    public class MerchantController : ApiController
    {
        private IClientManagerBLL _apiHelper;

        public MerchantController()
            : this(new ClientManagerBLL())
        {

        }

        public MerchantController(ClientManagerBLL apiHelper)
        {
            _apiHelper = apiHelper;
        }


        // GET api/merchant/top/6
        [HttpGet]
        [ActionName("top")]
        public dynamic GetTop(string amount)
        {
            if (string.IsNullOrWhiteSpace(amount))
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "商家个数不能为空"
                });
            }
            var isInt = new Regex(@"^\d+$", RegexOptions.Compiled | RegexOptions.Singleline)
                            .IsMatch(amount);
            if (!isInt)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "商家个数必须是整数"
                });
            }
            var count = Convert.ToInt32(amount.Trim());
            if (count < 1)
            {

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "商家个数必须大于0"
                });
            }
            var topn = _apiHelper.GetRecommendMerchants(count)
                        .Select(m => new
                                {
                                    Id = m.Id,
                                    Name = m.Name,
                                    ComprehensiveStar = m.ComprehensiveStar,
                                    Logo = "/api/merchant/logo/" + m.Id
                                })
                        .ToList();
            return Request.CreateResponse(HttpStatusCode.OK, new
                                        {
                                            success = true,
                                            msg = "请求成功",
                                            data = topn
                                        });
        }

        /// <summary>
        /// 不存在
        /// </summary>
        private string notExist = "not exist.jpg";
        /// <summary>
        /// 未认证
        /// </summary>
        private string unAuthenticated = "unauthenticated.jpg";
        /// <summary>
        /// 未授权
        /// </summary>
        private string unAuthorized = "unauthorized.jpg";

        // GET api/merchant/logo/123
        [HttpGet]
        [ActionName("logo")]
        public dynamic Logo(string id = null)
        {
            var merchant = _apiHelper.GetMerchantById(id);
            var resposne = Request.CreateResponse();

            if (merchant == null || string.IsNullOrWhiteSpace(id))
            {
                var path = HttpContext.Current.Server.MapPath("/Images/" + unAuthenticated);
                var fs = File.OpenRead(path);
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                resposne.Content = new ByteArrayContent(buffer);
            }
            else
            {
                if (merchant.Logo == null)
                {
                    var path = HttpContext.Current.Server.MapPath("/Images/" + notExist);
                    var fs = File.OpenRead(path);
                    var buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, (int)fs.Length);
                    resposne.Content = new ByteArrayContent(buffer);
                }
                else
                {
                    resposne.Content = new ByteArrayContent(merchant.Logo);
                }
            }
            resposne.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            resposne.StatusCode = HttpStatusCode.OK;
            return resposne;
        }

        // GET api/merchant/detail/5
        [HttpGet]
        [ActionName("detail")]
        public dynamic Get(string id = null)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "商家Id无效"
                });
            }
            var m = _apiHelper.GetMerchantById(id);
            if (m == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "商家Id无效"
                });
            }
            var merchant = new
            {
                Id = m.Id,
                Name = m.Name,
                Address = m.Address,
                Cellphone = m.Cellphone,
                Telephone = m.Telephone,
                QQ = m.QQ,
                ComprehensiveStar = m.ComprehensiveStar,
                Contacts = m.Contacts,
                Description = m.Description,
                RegisterCode = m.RegisterCode,
                Logo = "/api/merchant/logo?id=" + m.Id,
                SiteUrl = m.SiteUrl,
                //MerchantType = m.MerchantType.Name,
                Parent = m.ParentId
            };
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true,
                msg = "请求成功",
                data = merchant
            });
        }

        // GET api/merchant/products/233
        [HttpGet]
        [ActionName("products")]
        public dynamic GetProducts(string id, string pageIndex, string pageSize)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "商家Id不能为空"
                });
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
            var products = _apiHelper.GetServiceProductsByMerchantId(id, index, size);
            if (products.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    msg = "请求成功",
                    data = products.Select(p => new
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Star = p.Star,
                        Description = p.Description
                    }).ToArray()
                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    msg = "请求成功",
                    data = new string[0]
                });
            }
        }

        // GET api/merchant/messages/12341
        [HttpGet]
        [ActionName("messages")]
        public dynamic GetMessages(string id, string pageIndex, string pageSize)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return JsonConvert.SerializeObject(new
                {
                    success = false,
                    msg = "商家Id不能为空"
                });
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
            var messages = _apiHelper.GetformationsByMerchantId(id, index, size);
            if (messages == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "服务器发生异常"
                });
            }
            if (messages.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    msg = "请求成功",
                    data = messages.Select(m => new
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Content = m.Content,
                        CreateTime = m.CreateTime,
                        IsSend = m.IsSend,
                        IsRead = m.IsRead
                    }).ToArray()
                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    msg = "请求成功",
                    data = new string[0]
                });
            }
        }

        // GET api/merchant/messagedetail/1341341
        [HttpGet]
        [ActionName("report")]
        public dynamic GetMessageReport(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "信息Id不能为空"
                });
            }
            var report = _apiHelper.GetInformationFeedbackReport(id);
            if (report != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    msg = "请求成功",
                    data = report
                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "信息无反馈"
                });
            }
        }
    }
}
