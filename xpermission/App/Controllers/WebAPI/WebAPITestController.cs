using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace App.Controllers.WebAPI
{
    [HandleError]
    public class WebAPITestController : ApiController
    {
        // GET api/webapitest
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/webapitest/5
        public string Get(int id)
        {
            return id.ToString();
        }

        // POST api/webapitest
        public void Post([FromBody]string value)
        {
        }

        // PUT api/webapitest/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/webapitest/5
        public void Delete(int id)
        {
        }
    }
}
