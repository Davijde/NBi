using System.Data;
using System.Data.SqlClient;
using Moq;
using NBi.Core.ResultSet;
using NBi.NUnit.Query;
using NUnit.Framework;
using NBi.Core;
using NBi.Core.ResultSet.Equivalence;
using NBi.NUnit.ResultSetBased.Comparison;

namespace NBi.Testing.Unit.NUnit.Constraint.ResultSetBased.Comparison
{
    [TestFixture]
    public class SubsetOfConstraintTest
    {
        [Test]
        public void Matches_AnyServices_EachCalledOnce()
        {
            var rs = new ResultSet();
            rs.Load("a;b;c");

            var expected = Mock.Of<IResultSetService>();
            Mock.Get(expected).Setup(s => s.Execute()).Returns(rs);

            var actual = Mock.Of<IResultSetService>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(rs);

            var equivaler = Mock.Of<IEquivaler>();
            Mock.Get(equivaler).Setup(engine => engine.Compare(It.IsAny<ResultSet>(), It.IsAny<ResultSet>()))
                .Returns(new ResultResultSet() { Difference = ResultSetDifferenceType.None });

            var subsetOfConstraint = new SubsetOfConstraint(expected);
            subsetOfConstraint = subsetOfConstraint.Using(equivaler);

            //Method under test
            subsetOfConstraint.ApplyTo(actual);

            //Test conclusion            
            Mock.Get(equivaler).Verify(engine => engine.Compare(rs, rs), Times.Once());
            Mock.Get(expected).Verify(s => s.Execute(), Times.Once);
            Mock.Get(actual).Verify(s => s.Execute(), Times.Once);
        }

        [Test]
        public void Matches_AnyServices_TheirResultsAreCompared()
        {
            var expectedRs = new ResultSet();
            expectedRs.Load("a;b;c");

            var actualRs = new ResultSet();
            actualRs.Load("x;y;z");

            var expected = Mock.Of<IResultSetService>();
            Mock.Get(expected).Setup(s => s.Execute()).Returns(expectedRs);

            var actual = Mock.Of<IResultSetService>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var equivaler = Mock.Of<IEquivaler>();
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
            var rs = new ResultSet();
            rs.Load("a;b;c");

            var expected = Mock.Of<IResultSetService>();
            Mock.Get(expected).Setup(s => s.Execute()).Returns(rs);

            var actual = Mock.Of<IResultSetService>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(rs);

            var equivaler = Mock.Of<IEquivaler>();
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
            var expectedRs = new ResultSet();
            expectedRs.Load("a;b;c");

            var actualRs = new ResultSet();
            actualRs.Load("x;y;z");

            var expected = Mock.Of<IResultSetService>();
            Mock.Get(expected).Setup(s => s.Execute()).Returns(expectedRs);

            var actual = Mock.Of<IResultSetService>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var equivaler = Mock.Of<IEquivaler>();
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
