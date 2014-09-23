using System;

namespace Jarvis.MetadataService.Web.Support
{
    public static class Metadata
    {
        public static IMetadataProvider Provider { get; private set; }

        static Metadata()
        {
        }

        public static void Configure(string pathToCsvFolder)
        {
            Provider = new CsvMetadataProvider(
                pathToCsvFolder ??
                AppDomain.CurrentDomain.GetData("DataDirectory").ToString()
            );
        }
    }
}