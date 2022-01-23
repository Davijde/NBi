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
using NBi.Core.ResultSet.Filtering;
using NBi.Core.Calculation.Predication;
using NBi.Core.Calculation.Predicate.Text;

namespace NBi.Testing.NUnit.Messaging.Json.ResultSetBased
{
    public class RowCountFilteredMessengerJsonTest
    {

        #region Helpers
        private (IResultSet, IResultSet, RowCountFilterConstraintResult, RowCountFilteredMessengerJson) InitializeValues()
        {
            var predicationFilter = new PredicationFilter(new SinglePredication(new TextLowerCase(false), new ColumnNameIdentifier("foo")), null);
            var childConstraint = new LessThanConstraint(10);
            var childResult = childConstraint.ApplyTo(15);
            var constraint = new RowCountFilterPercentageConstraint(childConstraint, predicationFilter);
            var actual = new DataTableResultSet();
            actual.Load(new[]
                {
                new object[] { "foo", 100 }, new object[] { "foo", 120 },
                new object[] { "bar", 100 }, new object[] { "bar", 150 }
            });
            var filtered = new DataTableResultSet();
            filtered.Load(new[]
            {
                new object[] { "foo", 120 },
                new object[] { "bar", 100 }
            });
            var result = new RowCountFilterConstraintResult(constraint, actual, filtered, childResult);
            var msg = new RowCountFilteredMessengerJson(EngineStyle.ByIndex, new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default));
            return (actual, filtered, result, msg);
        }
        #endregion

        [Test]
        public void WriteAnalysis_LessThan_Constraint()
        {
            (_, var filtered, var result, var msg) = InitializeValues();
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                msg.WriteAnalysis(result, filtered, writer);
                var value = sb.ToString();
                Assert.That(value, Does.Contain($"\"constraint\":{{\"type\":\"LessThan\",\"description\":\"less than 10\""));
                Assert.That(value, Does.Contain($"\"threshold\":{{\"value\":10,\"unit\":\"absolute\",\"display\":\"10 rows\""));
            }
        }

        [Test]
        public void WriteAnalysis_LessThan_Result()
        {
            (_, var filtered, var result, var msg) = InitializeValues();
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                msg.WriteAnalysis(result, filtered, writer);
                var value = sb.ToString();
                Assert.That(value, Does.Contain($"\"result\":{{\"row-count\":15,\"message\":\""));
            }
        }

        [Test]
        public void WriteAnalysis_LessThan_Predicate()
        {
            (_, var filtered, var result, var msg) = InitializeValues();
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                msg.WriteAnalysis(result, filtered, writer);
                var value = sb.ToString();
                Assert.That(value, Does.Contain($"\"predication-filter\":{{\"total-rows\":2,\"table\":"));
            }
        }

        [Test]
        public void WriteMessage_ActualAndChildConstraint_ActualAndAnalysis()
        {
            (var actual, var filtered, var result, var msg) = InitializeValues();
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                var value = msg.WriteMessage(actual, filtered, result);
                Assert.That(value, Does.Contain($"\"actual\":"));
                Assert.That(value, Does.Contain($"\"analysis\":"));
                Assert.That(value, Does.Not.Contain($"\"expected\":"));
            }
        }
    }
}