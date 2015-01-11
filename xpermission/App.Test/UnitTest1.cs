using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text;
using System.IO;

namespace App.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var request = WebRequest.Create(new Uri("http://localhost:19582/api/account/login/8"));
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
