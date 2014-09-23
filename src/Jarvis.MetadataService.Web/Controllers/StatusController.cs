using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace Jarvis.MetadataService.Web.Controllers
{
    public class StatusController : ApiController
    {
        public object Get()
        {
            return new {
                name = Environment.MachineName, 
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                status = "ok"
            };
        }
    }
}
