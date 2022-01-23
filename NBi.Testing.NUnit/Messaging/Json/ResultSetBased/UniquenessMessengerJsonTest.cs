using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Discrimination;
using NBi.Extensibility;
using NBi.NUnit.Messaging.Json.ResultSetBased;
using NBi.Core.Sampling;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.NUnit.Messaging.Json.ResultSetBased
{
    public class UniquenessMessengerJsonTest
    {
        [Test]
        public void WriteAnalysis_ResultUniqueRows_TotalRowsIsDisplayed()
        {
            var result = new ResultUniqueRows(new List<KeyValuePair<KeyCollection, int>>()
                {
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "foo" }), 3),
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "bar" }), 5)
                });

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new UniquenessMessengerJson(EngineStyle.ByIndex, samplers);

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                msg.WriteAnalysis(result, writer);
                var value = sb.ToString();
                Assert.That(value, Does.Contain($"\"duplicated\":{{\"total-rows\":2"));
            }
        }

        [Test]
        public void WriteAnalysis_ResultUniqueRows_KeysAndOccurenceDisplayed()
        {
            var result = new ResultUniqueRows(new List<KeyValuePair<KeyCollection, int>>()
                {
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "foo" }), 3),
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "bar" }), 5)
                });

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new UniquenessMessengerJson(EngineStyle.ByIndex, samplers);

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                msg.WriteAnalysis(result, writer);
                var value = sb.ToString();
                Assert.That(value, Does.Contain($"Occurence"));
                Assert.That(value, Does.Contain($"\"3\",\"1\",\"foo\""));
                Assert.That(value, Does.Contain($"\"5\",\"1\",\"bar\""));
            }
        }

        [Test]
        public void WriteMessage_ActualAndResultUniqueRows_ActualAndAnalysis()
        {
            var actual = new DataTableResultSet();
            actual.Load(new[]
            {
                new object[] { "foo" }, new object[] { "foo" }, new object[] { "bar" }, new object[] { "bar" }
            });

            var result = new ResultUniqueRows(new List<KeyValuePair<KeyCollection, int>>()
                {
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "foo" }), 2),
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "bar" }), 2)
                });

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new UniquenessMessengerJson(EngineStyle.ByIndex, samplers);

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                var value = msg.WriteMessage(actual, result);
                Assert.That(value, Does.Contain($"\"actual\":"));
                Assert.That(value, Does.Contain($"\"analysis\":"));
                Assert.That(value, Does.Not.Contain($"\"expected\":"));
            }
        }
    }
}