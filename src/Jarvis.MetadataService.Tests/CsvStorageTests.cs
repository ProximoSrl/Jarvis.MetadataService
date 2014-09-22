using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jarvis.MetadataService.Web.Support;
using NUnit.Framework;

namespace Jarvis.MetadataService.Tests
{
    [TestFixture]
    public class CsvStorageTests
    {
        private CsvMetadataProvider _storage;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _storage = new CsvMetadataProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"));
        }

        [Test]
        public void asking_for_invalid_storeName_should_return_null()
        {
            var job1 = _storage.Get("invalid", "null");
            Assert.Null(job1);
        }

        [Test]
        public void asking_for_job1_on_jobStore_should_return_a_valid_dictionary()
        {
            var job1 = _storage.Get("jobs", "JOB1");
            Assert.NotNull(job1);
        }

        [Test]
        public void get_should_be_case_insensitive()
        {
            var joba = _storage.Get("jobs", "JOB1");
            var jobb = _storage.Get("jobs", "jOB1");
            Assert.NotNull(joba);
            Assert.NotNull(jobb);
            Assert.IsTrue(Object.ReferenceEquals(joba,jobb));
        }

        [Test]
        public void property_names_should_be_normalized()
        {
            var job1 = _storage.Get("jobs", "JOB1");
            
            Assert.AreEqual(4, job1.Keys.Count);
            Assert.AreEqual(job1["job"], "JOB1");
            Assert.AreEqual(job1["description"], "First job");
            Assert.AreEqual(job1["customer_id"], "PRXM");
            Assert.AreEqual(job1["company_name"], "Proximo s.r.l.");
        }
    }
}
