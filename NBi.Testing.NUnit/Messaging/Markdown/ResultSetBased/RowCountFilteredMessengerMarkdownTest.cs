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
    public class RowCountFilteredMessengerMarkdownTest
    {
        [Test]
        public void WriteMessage_NoRows_Predicate()
        {
            var predicationFilter = new PredicationFilter(new SinglePredication(new TextLowerCase(false), new ColumnNameIdentifier("foo")), null);
            var constraint = new NoRowsConstraint(predicationFilter);
            var childResult = new ConstraintResult(new EqualConstraint(0), 2, false);
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
            var result = new RowCountFilterConstraintResult(constraint, actual, filtered, childResult);
            
            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountFilteredMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var value = msg.WriteMessage(actual, filtered, result);
            Assert.That(value, Does.Contain($"Result-set has no row validating the predicate '[foo] is in small letters'."));
        }

        [Test]
        public void WriteMessage_AllRows_Predicate()
        {
            var predicationFilter = new PredicationFilter(new SinglePredication(new TextUpperCase(false), new ColumnNameIdentifier("foo")), null);
            var constraint = new AllRowsConstraint(predicationFilter);
            var childResult = new ConstraintResult(new EqualConstraint(0), 2, false);
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
            var result = new RowCountFilterConstraintResult(constraint, actual, filtered, childResult);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountFilteredMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var value = msg.WriteMessage(actual, filtered, result);
            Assert.That(value, Does.Contain($"Result-set has all rows validating the predicate '[foo] is in capital letters'."));
        }

        [Test]
        public void WriteMessage_SomeRows_Predicate()
        {
            var predicationFilter = new PredicationFilter(new SinglePredication(new TextUpperCase(false), new ColumnNameIdentifier("foo")), null);
            var constraint = new SomeRowsConstraint(predicationFilter);
            var childResult = new ConstraintResult(new EqualConstraint(0), 2, false);
            var actual = new DataTableResultSet();
            actual.Load(new[]
            {
                new object[] { "xyz", 100 }, new object[] { "abc", 120 },
                new object[] { "abc", 100 }, new object[] { "efg", 150 }
            });
            var filtered = new DataTableResultSet();
            filtered.Load(new[]
            {
                new object[] { "xyz", 100 }, new object[] { "abc", 120 },
                new object[] { "abc", 100 }, new object[] { "efg", 150 }
            });
            var result = new RowCountFilterConstraintResult(constraint, actual, filtered, childResult);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountFilteredMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var value = msg.WriteMessage(actual, filtered, result);
            Assert.That(value, Does.Contain($"Result-set has some rows validating the predicate '[foo] is in capital letters'."));
        }

        [Test]
        public void WriteMessage_SingleRow_Predicate()
        {
            var predicationFilter = new PredicationFilter(new SinglePredication(new TextUpperCase(false), new ColumnNameIdentifier("foo")), null);
            var constraint = new SingleRowConstraint(predicationFilter);
            var childResult = new ConstraintResult(new EqualConstraint(0), 2, false);
            var actual = new DataTableResultSet();
            actual.Load(new[]
            {
                new object[] { "XYZ", 100 }, new object[] { "abc", 120 },
                new object[] { "ABC", 100 }, new object[] { "efg", 150 }
            });
            var filtered = new DataTableResultSet();
            filtered.Load(new[]
            {
                new object[] { "XYZ", 100 },
                new object[] { "ABC", 100 }
            });
            var result = new RowCountFilterConstraintResult(constraint, actual, filtered, childResult);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountFilteredMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var value = msg.WriteMessage(actual, filtered, result);
            Assert.That(value, Does.Contain($"Result-set has a single row validating the predicate '[foo] is in capital letters'."));
        }

        [Test]
        public void WriteMessage_NoRows_Actual()
        {
            var predicationFilter = new PredicationFilter(new SinglePredication(new TextLowerCase(false), new ColumnNameIdentifier("foo")), null);
            var constraint = new NoRowsConstraint(predicationFilter);
            var childResult = new ConstraintResult(new EqualConstraint(0), 2, false);
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
            var result = new RowCountFilterConstraintResult(constraint, actual, filtered, childResult);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountFilteredMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var value = msg.WriteMessage(actual, filtered, result);
            Assert.That(value, Does.Contain($"4 rows"));
            Assert.That(value.Replace(" ",""), Does.Contain($"XYZ|100"));
        }

        [Test]
        public void WriteAnalysis_Filtered_Table()
        {
            var predicationFilter = new PredicationFilter(new SinglePredication(new TextLowerCase(false), new ColumnNameIdentifier("foo")), null);
            var constraint = new NoRowsConstraint(predicationFilter);
            var childResult = new ConstraintResult(new EqualConstraint(0), 2, false);
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
            var result = new RowCountFilterConstraintResult(constraint, actual, filtered, childResult);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountFilteredMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var writer = new StringBuilder();
            msg.WriteAnalysis(filtered, writer);
            var value = writer.ToString();
            Assert.That(value, Does.Contain($"2 rows"));
            Assert.That(value.Replace(" ", ""), Does.Contain($"abc|120"));
        }
    }

    //        [Test]
    //        public void WriteAnalysis_LessThan_Result()
    //        {
    //            var result = new LessThanConstraint(10).ApplyTo(15);
    //            var filtered = new DataTableResultSet();
    //            filtered.Load(new[]
    //            {
    //                new object[] { "foo", 100 }, new object[] { "foo", 120 },
    //                new object[] { "bar", 100 }, new object[] { "bar", 150 }
    //            });
    //            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
    //            var msg = new RowCountFilteredMessengerJson(EngineStyle.ByIndex, samplers);

    //            var sb = new StringBuilder();
    //            using (var sw = new StringWriter(sb))
    //            using (var writer = new JsonTextWriter(sw))
    //            {
    //                msg.WriteAnalysis(result, filtered, writer);
    //                var value = sb.ToString();
    //                Assert.That(value, Does.Contain($"\"result\":{{\"row-count\":15,\"message\":\""));
    //            }
    //        }

    //        [Test]
    //        public void WriteAnalysis_LessThan_Predicate()
    //        {
    //            var result = new LessThanConstraint(10).ApplyTo(15);
    //            var filtered = new DataTableResultSet();
    //            filtered.Load(new[]
    //            {
    //                new object[] { "foo", 100 }, new object[] { "foo", 120 },
    //                new object[] { "bar", 100 }, new object[] { "bar", 150 }
    //            });
    //            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
    //            var msg = new RowCountFilteredMessengerJson(EngineStyle.ByIndex, samplers);

    //            var sb = new StringBuilder();
    //            using (var sw = new StringWriter(sb))
    //            using (var writer = new JsonTextWriter(sw))
    //            {
    //                msg.WriteAnalysis(result, filtered, writer);
    //                var value = sb.ToString();
    //                Assert.That(value, Does.Contain($"\"predication-filter\":{{\"total-rows\":4,\"table\":"));
    //            }
    //        }

    //        [Test]
    //        public void WriteMessage_ActualAndChildConstraint_ActualAndAnalysis()
    //        {
    //            var result = new LessThanConstraint(10).ApplyTo(15);
    //            var actual = new DataTableResultSet();
    //            actual.Load(new[]
    //            {
    //                new object[] { "foo", 100 }, new object[] { "foo", 120 },
    //                new object[] { "bar", 100 }, new object[] { "bar", 150 }
    //            });
    //            var filtered = new DataTableResultSet();
    //            filtered.Load(new[]
    //            {
    //                new object[] { "foo", 120 }, new object[] { "bar", 150 }
    //            });
    //            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
    //            var msg = new RowCountFilteredMessengerJson(EngineStyle.ByIndex, samplers);

    //            var sb = new StringBuilder();
    //            using (var sw = new StringWriter(sb))
    //            using (var writer = new JsonTextWriter(sw))
    //            {
    //                var value = msg.WriteMessage(actual, filtered, result);
    //                Assert.That(value, Does.Contain($"\"actual\":"));
    //                Assert.That(value, Does.Contain($"\"analysis\":"));
    //                Assert.That(value, Does.Not.Contain($"\"expected\":"));
    //            }
    //        }
}
