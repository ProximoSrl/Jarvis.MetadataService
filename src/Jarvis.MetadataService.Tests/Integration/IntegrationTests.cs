using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Jarvis.MetadataService.Web;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Jarvis.MetadataService.Tests.Integration
{
    [TestFixture]
    public class IntegrationTests
    {
        IDisposable _app;
        HttpClient _client;
        static readonly Uri Endpoint = new Uri("http://localhost:5555");

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _app = WebApp.Start<SelfHostStartup>(Endpoint.AbsoluteUri);
            _client = new HttpClient();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _client.Dispose();
            _app.Dispose();
        }

        [Test]
        public void get_status_should_return_machine_and_version()
        {
            using (var response = _client.GetStringAsync(GetService("/status")))
            {
                var obj = JsonConvert.DeserializeObject<Dictionary<string,object>>(response.Result);
                Assert.AreEqual(Environment.MachineName, obj["MachineName"]);
                Assert.AreEqual(typeof(Startup).Assembly.GetName().Version.ToString(), obj["Version"]);
            }
        }

        Uri GetService(string service)
        {
            return new Uri(Endpoint, service);
        }
    }
}
