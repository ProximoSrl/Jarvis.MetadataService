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

namespace Jarvis.MetadataService.Providers
{
    class SqlStore
    {
        public string Name { get; set; }
        public string Query { get; set; }
        public string ConnectionString { get; set; }
    }

    class SqlQueryProvider : IMetadataProvider
    {
        private readonly IDictionary<string, SqlStore> _stores = new Dictionary<string, SqlStore>();

        public SqlQueryProvider(string sqlStore)
        {
            var json = JsonConvert.DeserializeObject<SqlStore[]>(File.ReadAllText(sqlStore));
            foreach (var store in json)
            {
                _stores.Add(store.Name.ToLowerInvariant(), store);
            }
        }

        public IDictionary<string, string> Get(string storeName, string key)
        {
            SqlStore store;
            if (!_stores.TryGetValue(storeName.ToLowerInvariant(), out store))
            {
                throw new Exception("Invalid store name " + storeName);
            }

            using (var connection = new SqlConnection(store.ConnectionString))
            using (SqlCommand command = new SqlCommand(store.Query, connection))
            {
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@id",
                    Value = key
                });

                var result = new Dictionary<string, string>();

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        var dataTable = reader.GetSchemaTable();

                        foreach (DataRow row in dataTable.Rows)
                        {
                            var cname = row.Field<string>("ColumnName");
                            var ordinal = row.Field<int>("ColumnOrdinal");

                            result.Add(
                                cname.ToLowerInvariant(),
                                reader[ordinal]?.ToString()
                            );
                        }
                    }

                    return result;
                }
            }
        }

        public string[] GetStoreNames()
        {
            return _stores.Keys.ToArray();
        }
    }
}
