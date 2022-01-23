using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Discrimination;
using NBi.Extensibility;
using NBi.NUnit.Messaging.Json.ResultSetBased;
using NBi.NUnit.Messaging.Markdown.ResultSetBased;
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

namespace NBi.Testing.NUnit.Messaging.Markdown.ResultSetBased
{
    public class UniquenessMessengerMarkdownTest
    {
        [Test]
        public void WriteMessage_ResultUniqueRows_Predicate()
        {
            var actual = new DataTableResultSet();
            actual.Load(new[]
                {
                    new object[] { 1, "foo" },
                    new object[] { 1, "bar" }
                });
            var result = new ResultUniqueRows(new List<KeyValuePair<KeyCollection, int>>()
                {
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "foo" }), 3),
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "bar" }), 5)
                });

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new UniquenessMessengerMarkdown(EngineStyle.ByIndex, samplers);

            var value = msg.WriteMessage(actual, result);
            Assert.That(value, Does.Contain($"All rows are unique"));
        }

        [Test]
        public void WriteAnalysis_ResultUniqueRows_TotalRowsIsDisplayed()
        {
            var result = new ResultUniqueRows(new List<KeyValuePair<KeyCollection, int>>()
                {
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "foo" }), 3),
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "bar" }), 5)
                });

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new UniquenessMessengerMarkdown(EngineStyle.ByIndex, samplers);

            var writer = new StringBuilder();
            msg.WriteAnalysis(result, writer);
            Assert.That(writer.ToString(), Does.Contain($"2 sets of rows are not unique"));
        }

        [Test]
        public void WriteAnalysis_ResultUniqueRows_DuplicatedIsDisplayed()
        {
            var result = new ResultUniqueRows(new List<KeyValuePair<KeyCollection, int>>()
                {
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "foo" }), 3),
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "bar" }), 5)
                });

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new UniquenessMessengerMarkdown(EngineStyle.ByIndex, samplers);

            var writer = new StringBuilder();
            msg.WriteAnalysis(result, writer);
            Assert.That(writer.ToString(), Does.Contain($"Duplicated rows:"));
        }

        [Test]
        public void WriteAnalysis_ResultUniqueRows_Occurence()
        {
            var result = new ResultUniqueRows(new List<KeyValuePair<KeyCollection, int>>()
                {
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "foo" }), 3),
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "bar" }), 5)
                });

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new UniquenessMessengerMarkdown(EngineStyle.ByIndex, samplers);

            var writer = new StringBuilder();
            msg.WriteAnalysis(result, writer);
            Assert.That(writer.ToString(), Does.Contain($"Occurence"));
        }

        [Test]
        public void WriteAnalysis_ResultUniqueRows_ResultSetRows()
        {
            var result = new ResultUniqueRows(new List<KeyValuePair<KeyCollection, int>>()
                {
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "foo" }), 3),
                    new KeyValuePair<KeyCollection, int>(new KeyCollection(new object[] { 1, "bar" }), 5)
                });

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new UniquenessMessengerMarkdown(EngineStyle.ByIndex, samplers);

            var writer = new StringBuilder();
            msg.WriteAnalysis(result, writer);
            Assert.That(writer.ToString().Replace(" ", ""), Does.Contain($"3|1|foo"));
            Assert.That(writer.ToString().Replace(" ", ""), Does.Contain($"5|1|bar"));
        }
    }
}