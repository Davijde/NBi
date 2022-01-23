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
using NBi.Core.ResultSet.Filtering;
using NBi.Core.Calculation.Predication;
using NBi.Core.Calculation.Predicate.Text;
using NBi.NUnit.ResultSetBased.RowPredicate;

namespace NBi.Testing.NUnit.Messaging.Json.ResultSetBased
{
    public class RowCountFilteredPercentageMessengerJsonTest
    {
        #region Helpers
        private (IResultSet, IResultSet, RowCountFilterPercentageConstraintResult, RowCountFilteredPercentageMessengerJson) InitializeValues()
        {
            var predicationFilter = new PredicationFilter(new SinglePredication(new TextLowerCase(false), new ColumnNameIdentifier("foo")), null);
            var childConstraint = new LessThanConstraint(0.25);
            var childResult = childConstraint.ApplyTo(0.5);
            var constraint = new RowCountFilterPercentageConstraint(childConstraint, predicationFilter);
            var actual = new DataTableResultSet();
            actual.Load(new[]
                {
                new object[] { "foo", 100 }, new object[] { "foo", 120 },
                new object[] { "bar", 100 }, new object[] { "bar", 150 }
            });
            var filter = new DataTableResultSet();
            filter.Load(new[]
                {
                new object[] { "foo", 120 },
                new object[] { "bar", 150 }
            });
            var result = new RowCountFilterPercentageConstraintResult(constraint, actual, filter, childResult);
            var msg = new RowCountFilteredPercentageMessengerJson(EngineStyle.ByIndex, new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default)
);
            return (actual, filter, result, msg);
        }
        #endregion

        [Test]
        public void WriteMessage_25PercentageRows_Threshold()
        {
            (var actual, var filtered, var result, var msg) = InitializeValues();
            var value = msg.WriteMessage(actual, filtered, result);
            Assert.That(value, Does.Contain($"\"threshold\":{{"));
            Assert.That(value, Does.Contain($"\"value\":25"));
            Assert.That(value, Does.Contain($"\"unit\":\"%\""));
            Assert.That(value, Does.Contain($"\"display\":\"25.00%\""));
        }
    }
}