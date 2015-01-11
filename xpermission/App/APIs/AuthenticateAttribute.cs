using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
//using System.Web.Mvc;

namespace App.APIs
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class AuthenticateAttribute : FilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request">Http请求</param>
        public AuthenticateAttribute(HttpRequestWrapper request)
        { 
        
        }
    }
}