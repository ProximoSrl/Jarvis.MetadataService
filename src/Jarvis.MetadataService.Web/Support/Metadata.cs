using System;

namespace Jarvis.MetadataService.Web.Support
{
    public static class Metadata
    {
        public static IMetadataProvider Provider { get; private set; }

        static Metadata()
        {
        }

        public static void Configure()
        {
            Provider = new CsvMetadataProvider(
                AppDomain.CurrentDomain.GetData("DataDirectory").ToString()
            );
        }
    }
}