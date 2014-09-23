using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Jarvis.MetadataService.Web.Models;

namespace Jarvis.MetadataService.Web.Controllers
{
    public class StatusController : ApiController
    {
        public StatusModel Get()
        {
            return new StatusModel();
        }
    }
}
