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
        private CsvMetadataProvider _provider;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _provider = new CsvMetadataProvider(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"));
        }

        [Test]
        public void asking_for_invalid_storeName_should_return_null()
        {
            var job1 = _provider.Get("invalid", "null");
            Assert.Null(job1);
        }

        [Test]
        public void asking_for_job1_on_jobStore_should_return_a_valid_dictionary()
        {
            var job1 = _provider.Get("jobs", "JOB1");
            Assert.NotNull(job1);
        }

        [Test]
        public void get_should_be_case_insensitive()
        {
            var joba = _provider.Get("jobs", "JOB1");
            var jobb = _provider.Get("jobs", "jOB1");
            Assert.NotNull(joba);
            Assert.NotNull(jobb);
            Assert.IsTrue(Object.ReferenceEquals(joba,jobb));
        }

        [Test]
        public void property_names_should_be_normalized()
        {
            var job1 = _provider.Get("jobs", "JOB1");
            
            Assert.AreEqual(4, job1.Keys.Count);
            Assert.AreEqual(job1["job"], "JOB1");
            Assert.AreEqual(job1["description"], "First job");
            Assert.AreEqual(job1["customer_id"], "PRXM");
            Assert.AreEqual(job1["company_name"], "Proximo s.r.l.");
        }

        [Test]
        public void provider_should_have_two_stores()
        {
            var names = _provider.GetStoreNames();

            Assert.AreEqual(2, names.Count());
            Assert.IsTrue(names.Contains("jobs"));
            Assert.IsTrue(names.Contains("customers"));
        }

        [Test]
        public void keys_should_be_trimmed()
        {
            var joba = _provider.Get("jobs", "JOB5");
            var jobb = _provider.Get("jobs", " JOB5");
            var jobc = _provider.Get("jobs", "JOB5 ");
            Assert.NotNull(joba);
            Assert.NotNull(jobb);
            Assert.NotNull(jobc);

            Assert.IsTrue(Object.ReferenceEquals(joba,jobb));
            Assert.IsTrue(Object.ReferenceEquals(jobc,jobb));
        }
    }
}
