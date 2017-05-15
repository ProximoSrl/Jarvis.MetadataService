using System;
using System.Collections.Generic;

namespace Jarvis.MetadataService.Support
{
    public interface IMetadataProvider
    {
        IDictionary<string, Object> Get(string storeName, string key);

        string[] GetStoreNames();
    }
}