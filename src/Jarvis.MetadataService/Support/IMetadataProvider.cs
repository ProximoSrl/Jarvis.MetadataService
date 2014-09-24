using System.Collections.Generic;

namespace Jarvis.MetadataService.Support
{
    public interface IMetadataProvider
    {
        IDictionary<string, string> Get(string storeName, string key);
        string[] GetStoreNames();
    }
}