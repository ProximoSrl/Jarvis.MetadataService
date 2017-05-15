using Jarvis.MetadataService.Providers;
using Jarvis.MetadataService.Tests.Support;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis.MetadataService.Tests
{
    [TestFixture(@"[{
    ""name"": ""customers"",
    ""query"": ""select * from customers where id = @id"",
    ""connectionString"": ""{SQLCONNECTION}""
  }]", "SQL_TEST_CONNECTION", "System.Data.SqlClient")]
    [TestFixture(@"[{
    ""name"": ""customers"",
    ""query"": ""select * from customers where id = :id"",
    ""connectionString"": ""{SQLCONNECTION}"",
    ""providerName"" : ""Oracle.ManagedDataAccess.Client""
  }]", "ORACLE_TEST_CONNECTION", "Oracle.ManagedDataAccess.Client")]
    public class SqlStorageTest
    {
        SqlQueryProvider sut;
        private readonly String _rawDefinition;
        private readonly String _variableName;
        private readonly String _providerName;

        public SqlStorageTest(String rawDefinition, String variableName, String providerName)
        {
            _rawDefinition = rawDefinition;
            _variableName = variableName;
            _providerName = providerName;
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var connection = TestDb.InitializeTestDatabase(_variableName, _providerName);
            connection = connection.Replace("\\", "\\\\");
            sut = SqlQueryProvider.CreateFromJsonDefinition(_rawDefinition.Replace("{SQLCONNECTION}", connection));
        }

        [Test]
        public void Get_one_record_with_integer_key()
        {
            var record = sut.Get("customers", "1");
            Assert.That(record["id"], Is.EqualTo(1));
            Assert.That(record["name"], Is.EqualTo("alkampfer"));
            Assert.That(record["surname"], Is.EqualTo("The zoalord"));
        }

        [Test]
        public void Get_record_that_does_not_Exists_will_not_throw()
        {
            var record = sut.Get("customers", "-1");
            Assert.That(record.Count, Is.EqualTo(0));
        }

        [Test]
        public void Get_schema()
        {
            var schema = sut.Get("customers", "@schema");
            Assert.That(schema["id"], Is.EqualTo("int"));
            Assert.That(schema["name"], Is.EqualTo("string"));
            Assert.That(schema["surname"], Is.EqualTo("string"));
        }
    }
}
