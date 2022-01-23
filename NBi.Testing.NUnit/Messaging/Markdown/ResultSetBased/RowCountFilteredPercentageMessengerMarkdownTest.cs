using NBi.Core.Calculation.Predication;
using NBi.Core.Calculation.Predicate.Text;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Filtering;
using NBi.Core.Sampling;
using NBi.Extensibility;
using NBi.NUnit.Messaging.Markdown.ResultSetBased;
using NBi.NUnit.ResultSetBased.RowPredicate;
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
    public class RowCountFilteredPercentageMessengerMarkdownTest
    {
        [Test]
        public void WriteMessage_25PercentageRows_Predicate()
        {
            var predicationFilter = new PredicationFilter(new SinglePredication(new TextLowerCase(false), new ColumnNameIdentifier("foo")), null);
            var childConstraint = new LessThanConstraint(0.25);
            var constraint = new RowCountFilterPercentageConstraint(childConstraint, predicationFilter);
            var childResult = new ConstraintResult(childConstraint, 0.5, false);
            var actual = new DataTableResultSet();
            actual.Load(new[]
            {
                new object[] { "XYZ", 100 }, new object[] { "abc", 120 },
                new object[] { "ABC", 100 }, new object[] { "efg", 150 }
            });
            var filtered = new DataTableResultSet();
            filtered.Load(new[]
            {
                new object[] { "abc", 120 },
                new object[] { "efg", 150 }
            });
            var result = new RowCountFilterPercentageConstraintResult(constraint, actual, filtered, childResult);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountFilteredMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var value = msg.WriteMessage(actual, filtered, result);
            Assert.That(value, Does.Contain($"Result-set has less than 25.00% of rows validating the predicate '[foo] is in small letters'."));
        }
    }
}
