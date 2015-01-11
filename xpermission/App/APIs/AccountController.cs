using BLL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using IBLL;

namespace App.APIs
{
    public class AccountController : ApiController
    {
        IClientManagerBLL _iClientManagerBLL;
        public AccountController()
            : this(new ClientManagerBLL())
        {

        }

        public AccountController(ClientManagerBLL clientManagerBLL)
        {
            _iClientManagerBLL = clientManagerBLL;
        }

        // GET api/account/login/5
        [HttpGet]
        [ActionName("login")]
        public dynamic Login(string code, string imei)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "注册码不能为空"
                });
            }

            var codeFormatIsValid = new Regex(@"^[0-9a-zA-Z]{8}$", RegexOptions.Compiled | RegexOptions.Singleline).IsMatch(code);
            if (!codeFormatIsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "注册码无效"
                });
            }
            string msg = string.Empty;
            string welcomeInfo = string.Empty;
            var user = _iClientManagerBLL.GetUserByRegisterCode(code, imei, out msg, out welcomeInfo);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = msg,
                    welcomeInfo = welcomeInfo
                });
            }
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true,
                msg = "请求成功",
                welcomeInfo = welcomeInfo,
                data = new
                {
                    Id = user.Id,
                    Code = user.Code,
                    Name = user.Name,
                    Type = user.Type,
                    MemberType=user.MemberType
                }
            });
        }

        // GET api/account/logout/5
        [HttpGet]
        [ActionName("logout")]
        public dynamic Logout(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "注册码不能为空"
                });
            }
            var logoutSuccess = _iClientManagerBLL.SignOut(code);
            if (logoutSuccess)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = true,
                    msg = "退出成功"
                });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    msg = "退出失败"
                });
            }
        }


        // POST api/account
        [HttpGet]
        [ActionName("test")]
        public void Post([FromBody]string value)
        {
            var request = WebRequest.Create(new Uri("http://wwtan.gicp.net/api/account/login/8"));
            request.Method = "GET";
            request.Headers.Add("Authenticate-Name", "tester");
            request.Headers.Add("Authenticate-Key", "20130905 22:11:00");
            var response = request.GetResponse();
            var rs = response.GetResponseStream();
            var utf8 = Encoding.UTF8;
            var st = new StreamReader(rs, utf8);
            var data = st.ReadToEnd();
        }
    }
}
