using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Discrimination;
using NBi.Extensibility;
using NBi.NUnit.Messaging.Json.ResultSetBased;
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

namespace NBi.Testing.NUnit.Messaging.Json.ResultSetBased
{
    public class RowCountMessengerJsonTest
    {
        [Test]
        public void WriteAnalysis_LessThan_Constraint()
        {
            var constraint = new LessThanConstraint(10);
            var result = constraint.ApplyTo(15);
            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountMessengerJson(EngineStyle.ByIndex, samplers);

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                msg.WriteAnalysis(result, writer);
                var value = sb.ToString();
                Assert.That(value, Does.StartWith($"\"analysis\":{{\"constraint\":{{\"type\":\"LessThan\",\"description\":\"less than 10\"}}"));
            }
        }

        [Test]
        public void WriteAnalysis_LessThan_Result()
        {
            var constraint = new LessThanConstraint(10);
            var result = constraint.ApplyTo(15);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountMessengerJson(EngineStyle.ByIndex, samplers);

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                msg.WriteAnalysis(result, writer);
                var value = sb.ToString();
                Assert.That(value, Does.Contain($",\"result\":{{\"row-count\":15,\"message\":\""));
            }
        }

        [Test]
        public void WriteMessage_ActualAndChildConstraint_ActualAndAnalysis()
        {
            var actual = new DataTableResultSet();
            actual.Load(new[]
            {
                new object[] { "foo" }, new object[] { "foo" }, new object[] { "bar" }, new object[] { "bar" }
            });

            var result = new LessThanConstraint(10).ApplyTo(15);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountMessengerJson(EngineStyle.ByIndex, samplers);

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