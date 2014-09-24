using System.Web.Http;
using Jarvis.MetadataService.Models;

namespace Jarvis.MetadataService.Controllers
{
    public class StatusController : ApiController
    {
        public StatusModel Get()
        {
            return new StatusModel();
        }
    }
}
