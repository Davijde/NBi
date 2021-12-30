using System.Data;
using System.Data.SqlClient;
using Moq;
using NBi.Core.ResultSet;
using NBi.NUnit.Query;
using NUnit.Framework;
using NBi.Core;
using NBi.Core.ResultSet.Equivalence;
using NBi.NUnit.ResultSetBased.Comparison;
using NBi.Extensibility.Resolving;
using NBi.Extensibility;
using System.Collections.Generic;
using NBi.Core.ResultSet.Analyzer;
using System;
using System.Linq;

namespace NBi.Testing.NUnit.Constraint.ResultSetBased.Comparison
{
    [TestFixture]
    public class SubsetOfConstraintTest
    {
        [Test]
        public void Matches_AnyServices_EachCalledOnce()
        {
            var rs = new DataTableResultSet();
            rs.Load("a;b;c");

            var expected = Mock.Of<IResultSetResolver>();
            Mock.Get(expected).Setup(s => s.Execute()).Returns(rs);

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(rs);

            var equivaler = Mock.Of<IEquivaler>();
            Mock.Get(equivaler).Setup(engine => engine.Using(It.IsAny<IEnumerable<IRowsAnalyzer>>()))
                .Returns(equivaler);
            Mock.Get(equivaler).Setup(engine => engine.Compare(It.IsAny<IResultSet>(), It.IsAny<IResultSet>()))
                .Returns(new ResultResultSet() { Difference = ResultSetDifferenceType.None });

            var subsetOfConstraint = new SubsetOfConstraint(expected);
            subsetOfConstraint = subsetOfConstraint.Using(equivaler);

            //Method under test
            subsetOfConstraint.ApplyTo(actual);

            //Test conclusion            
            Func<IList<IRowsAnalyzer>, bool> CheckAnalyzers = (analyzers)
                => analyzers.Any(analyzer => analyzer is KeyMatchingRowsAnalyzer)
                && analyzers.Any(analyzer => analyzer is UnexpectedRowsAnalyzer)
                && analyzers.Count == 2;


            Mock.Get(equivaler).Verify(engine => engine.Using(
                It.Is<IEnumerable<IRowsAnalyzer>>(analyzers => CheckAnalyzers(analyzers.ToList())))
                , Times.Once());
            Mock.Get(equivaler).Verify(engine => engine.Compare(rs, rs), Times.Once());
            Mock.Get(expected).Verify(s => s.Execute(), Times.Once);
            Mock.Get(actual).Verify(s => s.Execute(), Times.Once);
        }

        [Test]
        public void Matches_AnyServices_TheirResultsAreCompared()
        {
            var expectedRs = new DataTableResultSet();
            expectedRs.Load("a;b;c");

            var actualRs = new DataTableResultSet();
            actualRs.Load("x;y;z");

            var expected = Mock.Of<IResultSetResolver>();
            Mock.Get(expected).Setup(s => s.Execute()).Returns(expectedRs);

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var equivaler = Mock.Of<IEquivaler>();
            Mock.Get(equivaler).Setup(engine => engine.Using(It.IsAny<IEnumerable<IRowsAnalyzer>>()))
                .Returns(equivaler);
            Mock.Get(equivaler).Setup(engine => engine.Compare(actualRs, expectedRs))
                .Returns(new ResultResultSet() { Difference = ResultSetDifferenceType.Content });

            var subsetOfConstraint = new SubsetOfConstraint(expected);
            subsetOfConstraint = subsetOfConstraint.Using(equivaler);

            //Method under test
            subsetOfConstraint.ApplyTo(actual);

            //Test conclusion            
            Mock.Get(equivaler).Verify(engine => engine.Compare(actualRs, expectedRs), Times.Once());
        }

        [Test]
        public void Matches_TwoIdenticalResultSets_ReturnTrue()
        {
            var rs = new DataTableResultSet();
            rs.Load("a;b;c");

            var expected = Mock.Of<IResultSetResolver>();
            Mock.Get(expected).Setup(s => s.Execute()).Returns(rs);

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(rs);

            var equivaler = Mock.Of<IEquivaler>();
            Mock.Get(equivaler).Setup(engine => engine.Using(It.IsAny<IEnumerable<IRowsAnalyzer>>()))
                .Returns(equivaler);
            Mock.Get(equivaler).Setup(engine => engine.Compare(rs, rs))
                .Returns(new ResultResultSet() { Difference = ResultSetDifferenceType.None });

            var subsetOfConstraint = new SubsetOfConstraint(expected);
            subsetOfConstraint = subsetOfConstraint.Using(equivaler);

            //Method under test
            var result = subsetOfConstraint.ApplyTo(actual);

            //Test conclusion            
            Assert.That(result, Is.TypeOf<ResultSetComparisonConstraintResult>());
            Assert.That((result as ResultSetComparisonConstraintResult).IsSuccess, Is.True);
        }

        [Test]
        public void Matches_TwoDifferentResultSets_ReturnFalse()
        {
            var expectedRs = new DataTableResultSet();
            expectedRs.Load("a;b;c");

            var actualRs = new DataTableResultSet();
            actualRs.Load("x;y;z");

            var expected = Mock.Of<IResultSetResolver>();
            Mock.Get(expected).Setup(s => s.Execute()).Returns(expectedRs);

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var equivaler = Mock.Of<IEquivaler>();
            Mock.Get(equivaler).Setup(engine => engine.Using(It.IsAny<IEnumerable<IRowsAnalyzer>>()))
                .Returns(equivaler);
            Mock.Get(equivaler).Setup(engine => engine.Compare(actualRs, expectedRs))
                .Returns(new ResultResultSet() { Difference = ResultSetDifferenceType.Content });

            var subsetOfConstraint = new SubsetOfConstraint(expected);
            subsetOfConstraint = subsetOfConstraint.Using(equivaler);

            //Method under test
            var result = subsetOfConstraint.ApplyTo(actual);

            //Test conclusion            
            Assert.That(result, Is.TypeOf<ResultSetComparisonConstraintResult>());
            Assert.That((result as ResultSetComparisonConstraintResult).IsSuccess, Is.False);
        }
    }
}
