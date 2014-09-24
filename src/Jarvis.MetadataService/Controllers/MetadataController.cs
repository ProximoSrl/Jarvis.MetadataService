using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jarvis.MetadataService.Models;
using Jarvis.MetadataService.Support;

namespace Jarvis.MetadataService.Controllers
{
    public class MetadataController : ApiController
    {
        [Route("metadata/stores")]
        [HttpGet]
        public string[] ListStores()
        {
            return Metadata.Provider.GetStoreNames();
        }

        [Route("metadata/{kind}/{*key}")]
        [HttpGet]
        public HttpResponseMessage  GetByKey(string kind, string key)
        {
            var data = Metadata.Provider.Get(kind, key);
            if (data != null)
                return Request.CreateResponse(HttpStatusCode.OK, data);

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "no data available");
        }

        [Route("metadata/query")]
        [HttpPost]
        public HttpResponseMessage PostQuery(MetadataRequest mdreq)
        {
            if (mdreq == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid request");

            return GetByKey(mdreq.Kind, mdreq.Key);
        }
    }
}
