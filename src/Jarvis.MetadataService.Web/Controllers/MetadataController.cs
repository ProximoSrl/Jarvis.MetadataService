using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using Jarvis.MetadataService.Web.Support;

namespace Jarvis.MetadataService.Web.Controllers
{
    public class MetadataController : ApiController
    {
        [Route("metadata/{store}/{id}")]
        [HttpGet]
        public HttpResponseMessage  Get(string store, string id)
        {
            var data = Metadata.Provider.Get(store, id);
            if (data != null)
                return Request.CreateResponse(HttpStatusCode.OK, data);

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "no data available");
        }
    }
}
