using BLL;
using DAL;
using IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace App.APIs
{
    public class MerchantTypeController : ApiController
    {
        private IClientManagerBLL _apiHelper;


        public MerchantTypeController()
            : this(new ClientManagerBLL())
        {

        }

        public MerchantTypeController(ClientManagerBLL apiHelper)
        {
            _apiHelper = apiHelper;
        }

        [HttpGet]
        [ActionName("list")]
        public dynamic GetAllType()
        {
            var typeList = _apiHelper.GetMerchantTypes(null, null);
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true,
                msg = "请求成功",
                data = typeList.Select(t => new
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description
                })
            });
        }

        // GET api/merchanttype/5
        [HttpGet]
        [ActionName("list")]
        public dynamic GetTypeList(string pageIndex, string pageSize)
        {
            int? index = null;
            int? size = null;
            if (!string.IsNullOrWhiteSpace(pageIndex) && !string.IsNullOrWhiteSpace(pageSize))
            {
                var intRegex = new Regex(@"^\d+$", RegexOptions.Compiled | RegexOptions.Singleline);
                if (!intRegex.IsMatch(pageIndex))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = false,
                        msg = "页码必须是整数"
                    });
                }

                index = Convert.ToInt32(pageIndex);
                if (index.Value < 1)
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
                size = Convert.ToInt32(pageSize);
                if (size.Value < 1)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = false,
                        msg = "每页记录数不能小于1"
                    });
                }
            }
            List<MerchantType> typeList;
            if (pageIndex == null || pageSize == null)
            {
                typeList = _apiHelper.GetMerchantTypes(null, null);
            }
            else
            {
                typeList = _apiHelper.GetMerchantTypes(index, size);
            }
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true,
                msg = "请求成功",
                data = typeList.Select(t => new
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description
                })
            });
        }

        // GET api/merchanttype/merchants/3
        [HttpGet]
        [ActionName("merchants")]
        public dynamic GetMerchants(string id, string pageIndex, string pageSize)
        {

            var intRegex = new Regex(@"^-?\d+$", RegexOptions.Compiled | RegexOptions.Singleline);
            if (string.IsNullOrWhiteSpace(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "商家类型Id不能为空"
                });

            }
            if (!intRegex.IsMatch(id))
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "商家类型Id无效"
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

         
            var merchants = _apiHelper.GetMerchantsByMerchantTypeId(Convert.ToInt32(id), index, size);
            if (merchants.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    msg = "请求成功",
                    data = merchants.Select(m => new
                    {
                        Id = m.Id,
                        Name = m.Name,
                        ComprehensiveStar = m.ComprehensiveStar,
                        Description = m.Description,
                        Logo = "/api/merchant/logo/" + m.Id //,
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
    }
}
