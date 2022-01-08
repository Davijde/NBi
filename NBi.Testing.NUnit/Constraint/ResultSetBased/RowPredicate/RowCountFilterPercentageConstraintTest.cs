using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NBi.Core.ResultSet.Filtering;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using NBi.NUnit.ResultSetBased.RowPredicate;
using NBi.NUnit;


namespace NBi.Testing.NUnit.Constraint.ResultSetBased.RowPredicate
{
    [TestFixture]
    public class RowCountFilterPercentageConstraintTest
    {
        [Test]
        public void ApplyTo_ResultSetResolver_CallToExecuteOnce()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());
            var actualRsRowCount = 5;
            Mock.Get(actualRs).Setup(s => s.RowCount).Returns(actualRsRowCount);

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var filteredRs = Mock.Of<IResultSet>();
            var filteredRsRowCount = 2;
            Mock.Get(filteredRs).Setup(s => s.RowCount).Returns(filteredRsRowCount);

            var filter = Mock.Of<IPredicateFilter>();
            Mock.Get(filter).Setup(f => f.Execute(It.IsAny<IResultSet>())).Returns(filteredRs);

            var childCtrResult = new ConstraintResult(null, actualRs, ConstraintStatus.Success);

            var resolver = Mock.Of<IScalarResolver<decimal>>();
            Mock.Get(resolver).Setup(r => r.Execute()).Returns(It.IsAny<decimal>());

            var childCtrMock = new Mock<DifferedConstraint>(typeof(GreaterThanConstraint), resolver);
            childCtrMock.Setup(c => c.ApplyTo(It.IsAny<decimal>())).Returns(childCtrResult);

            var rowCountFilterPctCtr = new RowCountFilterPercentageConstraint(childCtrMock.Object, filter);

            rowCountFilterPctCtr.ApplyTo(actual);
            Mock.Get(actual).Verify(a => a.Execute(), Times.Once());
        }

        [Test]
        public void ApplyTo_Filter_CallToExecuteOnce()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());
            var actualRsRowCount = 5;
            Mock.Get(actualRs).Setup(s => s.RowCount).Returns(actualRsRowCount);

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var filteredRs = Mock.Of<IResultSet>();
            var filteredRsRowCount = 2;
            Mock.Get(filteredRs).Setup(s => s.RowCount).Returns(filteredRsRowCount);

            var filter = Mock.Of<IPredicateFilter>();
            Mock.Get(filter).Setup(f => f.Execute(It.IsAny<IResultSet>())).Returns(filteredRs);

            var childCtrResult = new ConstraintResult(null, actualRs, ConstraintStatus.Success);

            var resolver = Mock.Of<IScalarResolver<decimal>>();
            Mock.Get(resolver).Setup(r => r.Execute()).Returns(It.IsAny<decimal>());

            var childCtrMock = new Mock<DifferedConstraint>(typeof(GreaterThanConstraint), resolver);
            childCtrMock.Setup(c => c.ApplyTo(It.IsAny<decimal>())).Returns(childCtrResult);

            var rowCountFilterPctCtr = new RowCountFilterPercentageConstraint(childCtrMock.Object, filter);

            rowCountFilterPctCtr.ApplyTo(actual);
            Mock.Get(filter).Verify(f => f.Execute(It.IsAny<IResultSet>()), Times.Once());
        }

        [Test]
        public void ApplyTo_FilteredResultSet_CallToRowCountOnce()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());
            var actualRsRowCount = 5;
            Mock.Get(actualRs).Setup(s => s.RowCount).Returns(actualRsRowCount);

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var filteredRs = Mock.Of<IResultSet>();
            var filteredRsRowCount = 2;
            Mock.Get(filteredRs).Setup(s => s.RowCount).Returns(filteredRsRowCount);

            var filter = Mock.Of<IPredicateFilter>();
            Mock.Get(filter).Setup(f => f.Execute(It.IsAny<IResultSet>())).Returns(filteredRs);

            var childCtrResult = new ConstraintResult(null, actualRs, ConstraintStatus.Success);

            var resolver = Mock.Of<IScalarResolver<decimal>>();
            Mock.Get(resolver).Setup(r => r.Execute()).Returns(It.IsAny<decimal>());

            var childCtrMock = new Mock<DifferedConstraint>(typeof(GreaterThanConstraint), resolver);
            childCtrMock.Setup(c => c.ApplyTo(It.IsAny<decimal>())).Returns(childCtrResult);

            var rowCountFilterPctCtr = new RowCountFilterPercentageConstraint(childCtrMock.Object, filter);

            rowCountFilterPctCtr.ApplyTo(actual);
            Mock.Get(filteredRs).Verify(rs => rs.RowCount, Times.Once());
        }

        [Test]
        public void ApplyTo_ChildConstraint_CallToApplyToOnce()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());
            var actualRsRowCount = 5;
            Mock.Get(actualRs).Setup(s => s.RowCount).Returns(actualRsRowCount);

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var filteredRs = Mock.Of<IResultSet>();
            var filteredRsRowCount = 2;
            Mock.Get(filteredRs).Setup(s => s.RowCount).Returns(filteredRsRowCount);

            var filter = Mock.Of<IPredicateFilter>();
            Mock.Get(filter).Setup(f => f.Execute(It.IsAny<IResultSet>())).Returns(filteredRs);

            var childCtrResult = new ConstraintResult(null, actualRs, ConstraintStatus.Success);

            var resolver = Mock.Of<IScalarResolver<decimal>>();
            Mock.Get(resolver).Setup(r => r.Execute()).Returns(0.25m);

            var childCtrMock = new Mock<DifferedConstraint>(typeof(GreaterThanConstraint), resolver);
            childCtrMock.Setup(c => c.ApplyTo(It.IsAny<decimal>())).Returns(childCtrResult);

            var rowCountFilterPctCtr = new RowCountFilterPercentageConstraint(childCtrMock.Object, filter);

            rowCountFilterPctCtr.ApplyTo(actual);
            childCtrMock.Verify(c => c.ApplyTo(filteredRsRowCount / Convert.ToDecimal(actualRsRowCount)), Times.Once());
        }

        [Test]
        public void ApplyTo_ChildIsSuccess_Success()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());
            var actualRsRowCount = 5;
            Mock.Get(actualRs).Setup(s => s.RowCount).Returns(actualRsRowCount);

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var filteredRs = Mock.Of<IResultSet>();
            var filteredRsRowCount = 2;
            Mock.Get(filteredRs).Setup(s => s.RowCount).Returns(filteredRsRowCount);

            var filter = Mock.Of<IPredicateFilter>();
            Mock.Get(filter).Setup(f => f.Execute(It.IsAny<IResultSet>())).Returns(filteredRs);

            var childCtrResult = new ConstraintResult(null, actualRs, ConstraintStatus.Success);

            var resolver = Mock.Of<IScalarResolver<decimal>>();
            Mock.Get(resolver).Setup(r => r.Execute()).Returns(0.25m);

            var childCtrMock = new Mock<DifferedConstraint>(typeof(GreaterThanConstraint), resolver);
            childCtrMock.Setup(c => c.ApplyTo(It.IsAny<decimal>())).Returns(childCtrResult);

            var rowCountFilterPctCtr = new RowCountFilterPercentageConstraint(childCtrMock.Object, filter);

            var result = rowCountFilterPctCtr.ApplyTo(actual);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Status, Is.EqualTo(ConstraintStatus.Success));
        }

        [Test]
        public void ApplyTo_ChildIsFailure_Failure()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());
            var actualRsRowCount = 5;
            Mock.Get(actualRs).Setup(s => s.RowCount).Returns(actualRsRowCount);

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var filteredRs = Mock.Of<IResultSet>();
            var filteredRsRowCount = 2;
            Mock.Get(filteredRs).Setup(s => s.RowCount).Returns(filteredRsRowCount);

            var filter = Mock.Of<IPredicateFilter>();
            Mock.Get(filter).Setup(f => f.Execute(It.IsAny<IResultSet>())).Returns(filteredRs);

            var childCtrResult = new ConstraintResult(null, actualRs, ConstraintStatus.Failure);

            var resolver = Mock.Of<IScalarResolver<decimal>>();
            Mock.Get(resolver).Setup(r => r.Execute()).Returns(0.25m);

            var childCtrMock = new Mock<DifferedConstraint>(typeof(GreaterThanConstraint), resolver);
            childCtrMock.Setup(c => c.ApplyTo(It.IsAny<decimal>())).Returns(childCtrResult);

            var rowCountFilterPctCtr = new RowCountFilterPercentageConstraint(childCtrMock.Object, filter);

            var result = rowCountFilterPctCtr.ApplyTo(actual);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Status, Is.EqualTo(ConstraintStatus.Failure));
        }

        [Test]
        public void ApplyTo_ActualValue_SetToActualResultSet()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());
            var actualRsRowCount = 5;
            Mock.Get(actualRs).Setup(s => s.RowCount).Returns(actualRsRowCount);

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var filteredRs = Mock.Of<IResultSet>();
            var filteredRsRowCount = 2;
            Mock.Get(filteredRs).Setup(s => s.RowCount).Returns(filteredRsRowCount);

            var filter = Mock.Of<IPredicateFilter>();
            Mock.Get(filter).Setup(f => f.Execute(It.IsAny<IResultSet>())).Returns(filteredRs);

            var childCtrResult = new ConstraintResult(null, actualRs, ConstraintStatus.Success);

            var resolver = Mock.Of<IScalarResolver<decimal>>();
            Mock.Get(resolver).Setup(r => r.Execute()).Returns(0.25m);

            var childCtrMock = new Mock<DifferedConstraint>(typeof(GreaterThanConstraint), resolver);
            childCtrMock.Setup(c => c.ApplyTo(It.IsAny<decimal>())).Returns(childCtrResult);

            var rowCountFilterPctCtr = new RowCountFilterPercentageConstraint(childCtrMock.Object, filter);

            var result = rowCountFilterPctCtr.ApplyTo(actual);
            Assert.That(result.ActualValue, Is.Not.Null);
            Assert.That(result.ActualValue, Is.EqualTo(actualRs));
        }

    }
}
