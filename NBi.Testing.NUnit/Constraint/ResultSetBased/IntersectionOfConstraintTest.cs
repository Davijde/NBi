using System.Data;
using System.Data.SqlClient;
using Moq;
using NBi.Core.ResultSet;
using NBi.NUnit.Query;
using NUnit.Framework;
using NBi.Core;
using NBi.NUnit.ResultSetComparison;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet.Equivalence;

namespace NBi.Testing.Unit.NUnit.Constraint.ResultSetBased
{
    [TestFixture]
    public class IntersectionOfConstraintTest
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

            var intersectionOfConstraint = new IntersectionOfConstraint(expected);
            intersectionOfConstraint = intersectionOfConstraint.Using(equivaler);

            //Method under test
            intersectionOfConstraint.ApplyTo(actual);

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

            var intersectionOfConstraint = new IntersectionOfConstraint(expected);
            intersectionOfConstraint = intersectionOfConstraint.Using(equivaler);

            //Method under test
            intersectionOfConstraint.ApplyTo(actual);

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

            var intersectionOfConstraint = new IntersectionOfConstraint(expected);
            intersectionOfConstraint = intersectionOfConstraint.Using(equivaler);

            //Method under test
            var result = intersectionOfConstraint.ApplyTo(actual);

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

            var intersectionOfConstraint = new IntersectionOfConstraint(expected);
            intersectionOfConstraint = intersectionOfConstraint.Using(equivaler);

            //Method under test
            var result = intersectionOfConstraint.ApplyTo(actual);

            //Test conclusion            
            Assert.That(result, Is.TypeOf<ResultSetComparisonConstraintResult>());
            Assert.That((result as ResultSetComparisonConstraintResult).IsSuccess, Is.False);
        }
    }
}
