﻿using System.Data;
using System.Data.SqlClient;
using Moq;
using NBi.Core.ResultSet;
using NBi.NUnit.ResultSetComparison;
using NUnit.Framework;
using NBi.Core;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet.Equivalence;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;

namespace NBi.Testing.Unit.NUnit.ResultSetComparison
{
    [TestFixture]
    public class EqualToConstraintTest
    {
        [Test]
        public void Matches_AnyServices_EachCalledOnce()
        {
            var rs = new DataTableResultSet();
            rs.Load("a;b;c");

            var expectedServiceMock = new Mock<IResultSetResolver>();
            expectedServiceMock.Setup(s => s.Execute())
                .Returns(rs);
            var expectedService = expectedServiceMock.Object;

            var actualServiceMock = new Mock<IResultSetResolver>();
            actualServiceMock.Setup(s => s.Execute())
                .Returns(rs);
            var actualService = actualServiceMock.Object;

            var rscMock = new Mock<IEquivaler>();
            rscMock.Setup(engine => engine.Compare(It.IsAny<IResultSet>(), It.IsAny<IResultSet>()))
                .Returns(new ResultResultSet() { Difference = ResultSetDifferenceType.None });

            var equalToConstraint = new EqualToConstraint(expected);
            equalToConstraint = equalToConstraint.Using(equivaler);

            //Method under test
            equalToConstraint.ApplyTo(actual);

            //Test conclusion            
            rscMock.Verify(engine => engine.Compare(It.IsAny<IResultSet>(), It.IsAny<IResultSet>()), Times.Once());
            expectedServiceMock.Verify(s => s.Execute(), Times.Once);
            actualServiceMock.Verify(s => s.Execute(), Times.Once);
        }

        [Test]
        public void Matches_AnyServices_TheirResultsAreCompared()
        {
            var expectedRs = new DataTableResultSet();
            expectedRs.Load("a;b;c");

            var actualRs = new DataTableResultSet();
            actualRs.Load("x;y;z");

            var expectedServiceMock = new Mock<IResultSetResolver>();
            expectedServiceMock.Setup(s => s.Execute())
                .Returns(expectedRs);
            var expectedService = expectedServiceMock.Object;

            var actualServiceMock = new Mock<IResultSetResolver>();
            actualServiceMock.Setup(s => s.Execute())
                .Returns(actualRs);
            var actualService = actualServiceMock.Object;

            var rscMock = new Mock<IEquivaler>();
            rscMock.Setup(engine => engine.Compare(It.IsAny<IResultSet>(), It.IsAny<IResultSet>()))
                .Returns(new ResultResultSet() { Difference = ResultSetDifferenceType.Content });

            var equalToConstraint = new EqualToConstraint(expected);
            equalToConstraint = equalToConstraint.Using(equivaler);

            //Method under test
            equalToConstraint.ApplyTo(actual);

            //Test conclusion            
            Mock.Get(equivaler).Verify(engine => engine.Compare(actualRs, expectedRs), Times.Once());
        }

        [Test]
        public void Matches_TwoIdenticalResultSets_ReturnTrue()
        {
            var rs = new DataTableResultSet();
            rs.Load("a;b;c");

            var expectedServiceMock = new Mock<IResultSetResolver>();
            expectedServiceMock.Setup(s => s.Execute())
                .Returns(rs);
            var expectedService = expectedServiceMock.Object;

            var actualServiceMock = new Mock<IResultSetResolver>();
            actualServiceMock.Setup(s => s.Execute())
                .Returns(rs);
            var actualService = actualServiceMock.Object;

            var equivaler = Mock.Of<IEquivaler>();
            Mock.Get(equivaler).Setup(engine => engine.Compare(rs, rs))
                .Returns(new ResultResultSet() { Difference = ResultSetDifferenceType.None });

            var equalToConstraint = new EqualToConstraint(expected);
            equalToConstraint = equalToConstraint.Using(equivaler);
            //Method under test
            var result = equalToConstraint.ApplyTo(actual);

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

            var expectedServiceMock = new Mock<IResultSetResolver>();
            expectedServiceMock.Setup(s => s.Execute())
                .Returns(expectedRs);
            var expectedService = expectedServiceMock.Object;

            var actualServiceMock = new Mock<IResultSetResolver>();
            actualServiceMock.Setup(s => s.Execute())
                .Returns(actualRs);
            var actualService = actualServiceMock.Object;

            var equivaler = Mock.Of<IEquivaler>();
            Mock.Get(equivaler).Setup(engine => engine.Compare(actualRs, expectedRs))
                .Returns(new ResultResultSet() { Difference = ResultSetDifferenceType.Content });

            var equalToConstraint = new EqualToConstraint(expected);
            equalToConstraint = equalToConstraint.Using(equivaler);
            //Method under test
            var result = equalToConstraint.ApplyTo(actual);

            //Test conclusion            
            Assert.That(result, Is.TypeOf<ResultSetComparisonConstraintResult>());
            Assert.That((result as ResultSetComparisonConstraintResult).IsSuccess, Is.False);
        }
        
    }
}
