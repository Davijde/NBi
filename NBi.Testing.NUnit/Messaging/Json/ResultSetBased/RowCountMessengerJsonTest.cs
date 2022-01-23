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
using NBi.NUnit.ResultSetBased.RowPredicate;

namespace NBi.Testing.NUnit.Messaging.Json.ResultSetBased
{
    public class RowCountMessengerJsonTest
    {

        #region Helpers
        private (IResultSet, RowCountConstraintResult, RowCountMessengerJson) InitializeValues()
        {
            var childConstraint = new LessThanConstraint(10);
            var childResult = childConstraint.ApplyTo(15);
            var constraint = new RowCountConstraint(childConstraint);
            var actual = new DataTableResultSet();
            actual.Load(new[]
                {
                new object[] { "foo", 100 }, new object[] { "foo", 120 },
                new object[] { "bar", 100 }, new object[] { "bar", 150 }
            });
            var result = new RowCountConstraintResult(constraint, actual, childResult);
            var msg = new RowCountMessengerJson(EngineStyle.ByIndex, new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default));
            return (actual, result, msg);
        }
        #endregion

        [Test]
        public void WriteAnalysis_LessThan_Constraint()
        {
            (_, var result, var msg) = InitializeValues();
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                msg.WriteAnalysis(result, writer);
                var value = sb.ToString();
                Assert.That(value, Does.Contain($"\"constraint\":{{\"type\":\"LessThan\",\"description\":\"less than 10\""));
                Assert.That(value, Does.Contain($"\"threshold\":{{\"value\":10,\"unit\":\"absolute\",\"display\":\"10 rows\""));
            }
        }

        [Test]
        public void WriteAnalysis_LessThan_Result()
        {
            (_, var result, var msg) = InitializeValues();
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
            (var actual, var result, var msg) = InitializeValues();
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