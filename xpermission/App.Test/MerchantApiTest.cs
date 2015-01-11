using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL;
using System.Web;
using System.Net;
using System.IO;
using System.Text;

namespace App.Test
{
    [TestClass]
    public class MerchantApiTest
    {


        [TestMethod]
        public void Get()
        {
            var request = WebRequest.Create(new Uri("http://localhost:19582/api/merchant/get/2013082521275310634782d8b8546e4"));
            request.Method = "GET";
            request.Headers.Add("Authenticate-Name", "tester");
            request.Headers.Add("Authenticate-Key", "2013082521275310634782d8b8546e4");
            var response = request.GetResponse();
            var rs = response.GetResponseStream();
            var utf8 = Encoding.UTF8;
            var sr = new StreamReader(rs, utf8);
            var data = sr.ReadToEnd();
            Console.Write(data);
        }


        public void Post(Merchant merchant)
        {

        }
    }
}
