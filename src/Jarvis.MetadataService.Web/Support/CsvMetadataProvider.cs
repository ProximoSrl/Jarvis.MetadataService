using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CsvHelper;
using CsvHelper.Configuration;

namespace Jarvis.MetadataService.Web.Support
{
    public class CsvMetadataProvider
    {
        private static IDictionary<string, IDictionary<string, IDictionary<string,string>>> _store;

        public static IDictionary<string, string> Get(string store, string key)
        {
            return null;
        }

        public static void Load()
        {
            _store = new Dictionary<string, IDictionary<string, IDictionary<string, string>>>();
            var folder = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

            foreach (var pathToCsv in Directory.GetFiles(folder, "*.csv"))
            {
                using (var textReader = File.OpenText(pathToCsv))
                {
                    var fileDictionary = new Dictionary<string, IDictionary<string, string>>();
                    _store[Path.GetFileNameWithoutExtension(pathToCsv)] = fileDictionary;

                    using (var csvReader = new CsvReader(textReader,new CsvConfiguration()
                    {
                        Delimiter = ";"
                    }))
                    {
                        while (csvReader.Read())
                        {
                            var data = new Dictionary<string, string>();
                            for (int i = 0; i < csvReader.Parser.FieldCount; i++)
                            {
                                data[csvReader.FieldHeaders[i]] = csvReader.GetField(i);
                            }

                            string key = csvReader.GetField("[JOB]");

                            fileDictionary[key] = data;
                        }
                    }
                }
            }
        }
    }
}