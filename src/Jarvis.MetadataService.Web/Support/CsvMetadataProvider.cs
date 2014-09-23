using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CsvHelper;
using CsvHelper.Configuration;

namespace Jarvis.MetadataService.Web.Support
{
    public class CsvMetadataProvider : IMetadataProvider
    {
        private IDictionary<string, IDictionary<string, IDictionary<string, string>>> _parsedFiles;

        public IDictionary<string, string> Get(string storeName, string key)
        {
            IDictionary<string, IDictionary<string, string>> store;
            if (!_parsedFiles.TryGetValue(storeName.ToLowerInvariant().Trim(), out store))
            {
                return null;
            }

            IDictionary<string, string> value;
            if (!store.TryGetValue(key.ToLowerInvariant().Trim(), out value))
            {
                return null;
            }

            return value;
        }

        public CsvMetadataProvider(string folder)
        {
            LoadFolder(folder);
        }

        private void LoadFolder(string folder)
        {
            _parsedFiles = new Dictionary<string, IDictionary<string, IDictionary<string, string>>>();

            foreach (var pathToCsv in Directory.GetFiles(folder, "*.csv"))
            {
                using (var textReader = File.OpenText(pathToCsv))
                {
                    var fileDictionary = new Dictionary<string, IDictionary<string, string>>();
                    string fname = Path.GetFileNameWithoutExtension(pathToCsv).ToLowerInvariant();
                    _parsedFiles[fname] = fileDictionary;

                    using (var csvReader = new CsvReader(textReader, new CsvConfiguration()
                    {
                        Delimiter = ";"
                    }))
                    {
                        bool addDiscoInfo = true;
                        while (csvReader.Read())
                        {
                            if (addDiscoInfo)
                            {
                                var schema = new Dictionary<string, string>();
                                foreach (var fieldHeader in csvReader.FieldHeaders)
                                {
                                    schema.Add(MakePropertyName(fieldHeader), "string");
                                }

                                fileDictionary["@schema"] = schema;

                                addDiscoInfo = false;
                            }

                            var data = new Dictionary<string, string>();
                            for (int i = 0; i < csvReader.Parser.FieldCount; i++)
                            {
                                string fieldHeader = MakePropertyName(csvReader.FieldHeaders[i]);
                                data[fieldHeader] = csvReader.GetField(i).Trim();
                            }

                            string key = csvReader.GetField(csvReader.FieldHeaders[0]).ToLowerInvariant().Trim();

                            fileDictionary[key] = data;
                        }
                    }
                }
            }
        }

        private string MakePropertyName(string header)
        {
            return header
                .Replace("[", "")
                .Replace("]", "")
                .Trim()
                .Replace(" ", "_")
                .ToLowerInvariant();
        }

        public string[] GetStoreNames()
        {
            return _parsedFiles.Keys.ToArray();
        }
    }
}