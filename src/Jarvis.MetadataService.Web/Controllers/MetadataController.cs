using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jarvis.MetadataService.Web.Support;

namespace Jarvis.MetadataService.Web.Controllers
{
    public class MetadataController : ApiController
    {
        // GET: api/Metadata
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Metadata/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Metadata
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Metadata/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Metadata/5
        public void Delete(int id)
        {
        }
    }
}
