using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using NunitCtr = NUnit.Framework.Constraints;
using NBi.Core.ResultSet.Filtering;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using NBi.NUnit.ResultSetBased.RowPredicate;

namespace NBi.Testing.NUnit.Constraint.ResultSetBased.RowPredicate
{
    [TestFixture]
    public class RowCountConstraintTest
    {
        [Test]
        public void ApplyTo_ResultSetResolver_CallToExecuteOnce()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var childCtr = Mock.Of<NunitCtr.Constraint>();
            var childCtrResult = new NunitCtr.ConstraintResult(childCtr, actualRs, NunitCtr.ConstraintStatus.Success);
            Mock.Get(childCtr).Setup(c => c.ApplyTo(It.IsAny<int>())).Returns(childCtrResult);

            var rowCountCtr = new RowCountConstraint(childCtr);

            rowCountCtr.ApplyTo(actual);
            Mock.Get(actual).Verify(a => a.Execute(), Times.Once());
        }

        [Test]
        public void ApplyTo_ChildConstraint_CallToApplyToOnce()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.RowCount).Returns(42);
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var childCtr = Mock.Of<NunitCtr.Constraint>();
            var childCtrResult = new NunitCtr.ConstraintResult(childCtr, actualRs, NunitCtr.ConstraintStatus.Success);
            Mock.Get(childCtr).Setup(c => c.ApplyTo(It.IsAny<int>())).Returns(childCtrResult);

            var rowCountCtr = new RowCountConstraint(childCtr);

            rowCountCtr.ApplyTo(actual);
            Mock.Get(childCtr).Verify(c => c.ApplyTo(It.Is<int>(x => x == 42)), Times.Once());
        }

        [Test]
        public void ApplyTo_ChildIsSuccess_Success()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var childCtr = Mock.Of<NunitCtr.Constraint>();
            var childCtrResult = new NunitCtr.ConstraintResult(childCtr, actualRs, NunitCtr.ConstraintStatus.Success);
            Mock.Get(childCtr).Setup(c => c.ApplyTo(It.IsAny<int>())).Returns(childCtrResult);

            var rowCountCtr = new RowCountConstraint(childCtr);

            var result = rowCountCtr.ApplyTo(actual);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Status, Is.EqualTo(NunitCtr.ConstraintStatus.Success));
        }

        [Test]
        public void ApplyTo_ChildIsFailure_Failure()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var childCtr = Mock.Of<NunitCtr.Constraint>();
            var childCtrResult = new NunitCtr.ConstraintResult(childCtr, actualRs, NunitCtr.ConstraintStatus.Failure);
            Mock.Get(childCtr).Setup(c => c.ApplyTo(It.IsAny<int>())).Returns(childCtrResult);

            var rowCountCtr = new RowCountConstraint(childCtr);

            var result = rowCountCtr.ApplyTo(actual);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Status, Is.EqualTo(NunitCtr.ConstraintStatus.Failure));
        }

        [Test]
        public void ApplyTo_ActualValue_SetToActualResultSet()
        {
            var actualRs = Mock.Of<IResultSet>();

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var filteredRs = Mock.Of<IResultSet>();
            Mock.Get(filteredRs).Setup(s => s.RowCount).Returns(0);

            var filter = Mock.Of<IPredicateFilter>();
            Mock.Get(filter).Setup(f => f.Execute(It.IsAny<IResultSet>())).Returns(filteredRs);

            var rowCountCtr = new NoRowsConstraint(filter);

            var result = rowCountCtr.ApplyTo(actual);
            Assert.That(result.ActualValue, Is.Not.Null);
            Assert.That(result.ActualValue, Is.EqualTo(actualRs));
        }

    }
}
