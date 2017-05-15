using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Jarvis.MetadataService.Support;
using System;

namespace Jarvis.MetadataService.Providers
{
    public class CsvMetadataProvider : IMetadataProvider
    {
        private IDictionary<string, IDictionary<string, IDictionary<string, Object>>> _parsedFiles;

        public IDictionary<string, Object> Get(string storeName, string key)
        {
            IDictionary<string, IDictionary<string, Object>> store;
            if (!_parsedFiles.TryGetValue(storeName.ToLowerInvariant().Trim(), out store))
            {
                return null;
            }

            IDictionary<string, Object> value;
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
            _parsedFiles = new Dictionary<string, IDictionary<string, IDictionary<string, Object>>>();

            foreach (var pathToCsv in Directory.GetFiles(folder, "*.csv"))
            {
                using (var streamReader = new StreamReader(pathToCsv, Encoding.Default))
                {
                    var fileDictionary = new Dictionary<string, IDictionary<string, Object>>();
                    string fname = Path.GetFileNameWithoutExtension(pathToCsv).ToLowerInvariant();
                    _parsedFiles[fname] = fileDictionary;

                    using (var csvReader = new CsvReader(streamReader, new CsvConfiguration()
                    {
                        Delimiter = ";"
                    }))
                    {
                        bool addDiscoInfo = true;
                        while (csvReader.Read())
                        {
                            if (addDiscoInfo)
                            {
                                var schema = new Dictionary<string, Object>();
                                foreach (var fieldHeader in csvReader.FieldHeaders)
                                {
                                    schema.Add(MakePropertyName(fieldHeader), "string");
                                }

                                fileDictionary["@schema"] = schema;

                                addDiscoInfo = false;
                            }

                            var data = new Dictionary<string, Object>();
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