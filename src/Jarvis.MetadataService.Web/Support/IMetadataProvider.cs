using System.Collections.Generic;

namespace Jarvis.MetadataService.Web.Support
{
    public interface IMetadataProvider
    {
        IDictionary<string, string> Get(string storeName, string key);
    }
}