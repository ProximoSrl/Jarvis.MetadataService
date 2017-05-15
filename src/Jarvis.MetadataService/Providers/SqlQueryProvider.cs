using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jarvis.MetadataService.Support;
using Newtonsoft.Json;
using Jarvis.MetadataService.Sql;
using System.Configuration;
using System.Diagnostics;

namespace Jarvis.MetadataService.Providers
{
    internal class SqlStore
    {
        public string Name { get; set; }
        public string Query { get; set; }
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }

        private ConnectionStringSettings _connectionStringSettings;

        public ConnectionStringSettings Connection
        {
            get
            {
                return _connectionStringSettings ?? (_connectionStringSettings =
                    new ConnectionStringSettings("connection", ConnectionString, ProviderName ?? "System.Data.SqlClient"));
            }
        }
    }

    internal class SqlQueryProvider : IMetadataProvider
    {
        private readonly IDictionary<string, SqlStore> _stores = new Dictionary<string, SqlStore>();

        public static SqlQueryProvider CreateFromFileDefinition(String fileDefinitionPath)
        {
            return new SqlQueryProvider(File.ReadAllText(fileDefinitionPath));
        }

        public static SqlQueryProvider CreateFromJsonDefinition(String serializedJson)
        {
            return new SqlQueryProvider(serializedJson);
        }

        private SqlQueryProvider(string serializedJson)
        {
            var json = JsonConvert.DeserializeObject<SqlStore[]>(serializedJson);
            foreach (var store in json)
            {
                _stores.Add(store.Name.ToLowerInvariant(), store);
            }
        }

        public IDictionary<string, Object> Get(string storeName, string key)
        {
            SqlStore store;
            if (!_stores.TryGetValue(storeName.ToLowerInvariant(), out store))
            {
                throw new Exception("Invalid store name " + storeName);
            }

            var result = new Dictionary<string, Object>(StringComparer.OrdinalIgnoreCase);

            if ("@schema".Equals(key, StringComparison.OrdinalIgnoreCase))
            {
                //We need to retrieve the schema, not the record
                DataAccess.CreateQueryOn(store.Connection, store.Query)
                    .SetParam("id", "")
                    .ExecuteGetSchema(schemaTable =>
                    {
                        foreach (DataRow myField in schemaTable.Rows)
                        {
                            result[myField["ColumnName"].ToString()] = GetType(myField["DataType"] as Type);
                        }
                    });
            }
            else
            {
                DataAccess.CreateQueryOn(store.Connection, store.Query)
                    .SetParam("id", key)
                    .ExecuteReaderMaxRecord(1, reader =>
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            result[reader.GetName(i)] = reader[i];
                        }
                    });
            }

            return result;
        }

        private object GetType(Type v)
        {
            if (v == typeof(String))
                return "string";
            if (v == typeof(Int32) || v == typeof(Int16) || v == typeof(Int64))
                return "int";
            if (v == typeof(Double) || v == typeof(Single) || v == typeof(Decimal))
                return "number";
            throw new NotSupportedException($"Unsupported type {v.FullName}");
        }

        public string[] GetStoreNames()
        {
            return _stores.Keys.ToArray();
        }
    }
}
