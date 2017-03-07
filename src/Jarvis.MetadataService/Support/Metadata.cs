using System;
using System.IO;
using Jarvis.MetadataService.Providers;
using log4net;

namespace Jarvis.MetadataService.Support
{
    public static class Metadata
    {
        public static IMetadataProvider Provider { get; private set; }
        static ILog Logger = log4net.LogManager.GetLogger(typeof(Metadata));

        static Metadata()
        {
        }

        public static void Configure(string pathToFolder)
        {
            var folder = pathToFolder ??
                         AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

            var sqlStore = Path.Combine(folder, "sqlstore.json");
            if (File.Exists(sqlStore))
            {
                Logger.Debug("Running on SqlQuery Provider");
                Provider = new SqlQueryProvider(sqlStore);
            }
            else
            {
                Logger.Debug("Running on CSV Provider");
                Provider = new CsvMetadataProvider(folder);
            }
        }
    }
}