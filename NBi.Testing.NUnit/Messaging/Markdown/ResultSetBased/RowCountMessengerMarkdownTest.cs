using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Discrimination;
using NBi.Extensibility;
using NBi.NUnit.Messaging.Json.ResultSetBased;
using NBi.NUnit.Messaging.Markdown.ResultSetBased;
using NBi.Core.Sampling;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.NUnit.Messaging.Markdown.ResultSetBased
{
    public class RowCountMessengerMarkdownTest
    {
        [Test]
        public void WriteMessage_LessThan_Constraint()
        {
            var constraint = new LessThanConstraint(10);
            var result = constraint.ApplyTo(15);
            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountMessengerMarkdown(EngineStyle.ByIndex, samplers);

            var value = msg.WriteMessage(new DataTableResultSet(), result);
            Assert.That(value, Does.Contain("Result-set has less than 10 rows"));
        }

        [Test]
        public void WriteMessage_Equal_Constraint()
        {
            var constraint = new EqualConstraint(10);
            var result = constraint.ApplyTo(15);
            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountMessengerMarkdown(EngineStyle.ByIndex, samplers);

            var value = msg.WriteMessage(new DataTableResultSet(), result);
            Assert.That(value, Does.Contain("Result-set has 10 rows"));
        }

        [Test]
        public void WriteMessage_GreaterThanOrEqual_Constraint()
        {
            var constraint = new GreaterThanOrEqualConstraint(10);
            var result = constraint.ApplyTo(5);
            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountMessengerMarkdown(EngineStyle.ByIndex, samplers);

            var value = msg.WriteMessage(new DataTableResultSet(), result);
            Assert.That(value, Does.Contain("Result-set has greater than or equal to 10 rows"));
        }

        [Test]
        public void WriteMessage_ResultSet_ActualRowsCountDisplayed()
        {
            var actual = new DataTableResultSet();
            actual.Load(new[]
                {
                    new object[] { 1, "foo" },
                    new object[] { 1, "bar" }
                });
            var result = new LessThanConstraint(10).ApplyTo(15);
            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountMessengerMarkdown(EngineStyle.ByIndex, samplers);

            var value = msg.WriteMessage(actual, result);
            Assert.That(value, Does.Contain($"Result-set with 2 rows"));
        }

        [Test]
        public void WriteMessage_ResultSet_RowsDisplayed()
        {
            var actual = new DataTableResultSet();
            actual.Load(new[]
                {
                    new object[] { 1, "foo" },
                    new object[] { 1, "bar" }
                });
            var result = new LessThanConstraint(10).ApplyTo(15);
            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountMessengerMarkdown(EngineStyle.ByIndex, samplers);

            var value = msg.WriteMessage(actual, result);
            Assert.That(value.Replace(" ", ""), Does.Contain($"1|foo"));
            Assert.That(value.Replace(" ", ""), Does.Contain($"1|bar"));
        }
    }
}