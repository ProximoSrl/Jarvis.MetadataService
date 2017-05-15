using Jarvis.MetadataService.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.MetadataService.Tests.Support
{
    public static class TestDb
    {
        public static ConnectionStringSettings ConnectionString { get; internal set; }

        public static String InitializeTestDatabase(String variableName, String providerName)
        {
            var connectionString = Environment.GetEnvironmentVariable(variableName);
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"Unable to find connection in {variableName}");
            }
            ConnectionString = new ConnectionStringSettings("test", connectionString, providerName);
            try
            {
                DataAccess.CreateQueryOn(ConnectionString, "DROP TABLE Customers").ExecuteNonQuery();
            }
            catch (Exception)
            {
                //Can tolerate drop table customers because it could not exists
            }

            if (providerName == "System.Data.SqlClient")
            {
                DataAccess.CreateQueryOn(ConnectionString, @"
CREATE TABLE [Customers](
	[id] [int] NOT NULL,
	[name] [nvarchar](50) NULL,
	[surname] [nvarchar](50) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
")
    .ExecuteNonQuery();
                InsertRecordSql(1, "alkampfer", "The zoalord");
                InsertRecordSql(2, "guardian", "The unique");
            }
            else if (providerName == "Oracle.ManagedDataAccess.Client")
            {
                DataAccess.CreateQueryOn(ConnectionString, @"
CREATE TABLE Customers (
         id      NUMBER(5) PRIMARY KEY,
         name      VARCHAR2(50) NOT NULL,
         surname    VARCHAR2(50) NOT NULL)")
                    .ExecuteNonQuery();
                InsertRecordOracle(1, "alkampfer", "The zoalord");
                InsertRecordOracle(2, "guardian", "The unique");
            }

            return ConnectionString.ConnectionString;
        }

        private static void InsertRecordSql(Int32 id, String name, String surname)
        {
            //Insert a couple of records
            DataAccess.CreateQueryOn(ConnectionString, @"
INSERT INTO [dbo].[Customers]
           ([id],
            [name]
           ,[surname])
     VALUES
           (@id, @name, @surname)")
           .SetInt32Param("id", id)
           .SetStringParam("name", name)
           .SetStringParam("surname", surname)
           .ExecuteNonQuery();
        }

        private static void InsertRecordOracle(Int32 id, String name, String surname)
        {
            //Insert a couple of records
            DataAccess.CreateQueryOn(ConnectionString, @"
INSERT INTO Customers
           (id,
            name,
            surname)
     VALUES
           (:id, :name, :surname)")
           .SetInt32Param("id", id)
           .SetStringParam("name", name)
           .SetStringParam("surname", surname)
           .ExecuteNonQuery();
        }
    }
}
