using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using NBi.Core.ResultSet.Filtering;
using NBi.Extensibility.Resolving;
using NBi.NUnit.ResultSetBased.RowPredicate;
using NBi.Extensibility;
using NUnit.Framework.Constraints;

namespace NBi.Testing.NUnit.Constraint.ResultSetBased.RowPredicate
{
    [TestFixture]
    public class SingleRowConstraintTest
    {
        [Test]
        public void ApplyTo_ResultSetResolver_CallToExecuteOnce()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var filteredRs = Mock.Of<IResultSet>();
            Mock.Get(filteredRs).Setup(s => s.RowCount).Returns(It.IsAny<int>());

            var filter = Mock.Of<IPredicateFilter>();
            Mock.Get(filter).Setup(f => f.Execute(It.IsAny<IResultSet>())).Returns(filteredRs);

            var singleRowCtr = new SingleRowConstraint(filter);

            singleRowCtr.ApplyTo(actual);
            Mock.Get(actual).Verify(a => a.Execute(), Times.Once());
        }

        [Test]
        public void ApplyTo_Filter_CallToExecuteOnce()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var filteredRs = Mock.Of<IResultSet>();
            Mock.Get(filteredRs).Setup(s => s.RowCount).Returns(It.IsAny<int>());

            var filter = Mock.Of<IPredicateFilter>();
            Mock.Get(filter).Setup(f => f.Execute(It.IsAny<IResultSet>())).Returns(filteredRs);

            var singleRowCtr = new SingleRowConstraint(filter);

            singleRowCtr.ApplyTo(actual);
            Mock.Get(filter).Verify(f => f.Execute(It.IsAny<IResultSet>()), Times.Once());
        }

        [Test]
        public void ApplyTo_FilteredResultSet_CallToRowCountOnce()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var filteredRs = Mock.Of<IResultSet>();
            Mock.Get(filteredRs).Setup(s => s.RowCount).Returns(It.IsAny<int>());

            var filter = Mock.Of<IPredicateFilter>();
            Mock.Get(filter).Setup(f => f.Execute(It.IsAny<IResultSet>())).Returns(filteredRs);

            var singleRowCtr = new SingleRowConstraint(filter);

            singleRowCtr.ApplyTo(actual);
            Mock.Get(filteredRs).Verify(rs => rs.RowCount, Times.Once());
        }

        [Test]
        public void ApplyTo_ResultZeroRow_Failure()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var filteredRs = Mock.Of<IResultSet>();
            Mock.Get(filteredRs).Setup(s => s.RowCount).Returns(0);

            var filter = Mock.Of<IPredicateFilter>();
            Mock.Get(filter).Setup(f => f.Execute(It.IsAny<IResultSet>())).Returns(filteredRs);

            var singleRowCtr = new SingleRowConstraint(filter);

            var result = singleRowCtr.ApplyTo(actual);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Status, Is.EqualTo(ConstraintStatus.Failure));
        }

        [Test]
        public void ApplyTo_ResultOneRow_Success()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var filteredRs = Mock.Of<IResultSet>();
            Mock.Get(filteredRs).Setup(s => s.RowCount).Returns(1);

            var filter = Mock.Of<IPredicateFilter>();
            Mock.Get(filter).Setup(f => f.Execute(It.IsAny<IResultSet>())).Returns(filteredRs);

            var singleRowCtr = new SingleRowConstraint(filter);

            var result = singleRowCtr.ApplyTo(actual);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Status, Is.EqualTo(ConstraintStatus.Success));
        }

        [Test]
        public void ApplyTo_ResultManyRows_Failure()
        {
            var actualRs = Mock.Of<IResultSet>();
            Mock.Get(actualRs).Setup(s => s.Rows).Returns(Enumerable.Empty<IResultRow>());

            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(actualRs);

            var filteredRs = Mock.Of<IResultSet>();
            Mock.Get(filteredRs).Setup(s => s.RowCount).Returns(1000);

            var filter = Mock.Of<IPredicateFilter>();
            Mock.Get(filter).Setup(f => f.Execute(It.IsAny<IResultSet>())).Returns(filteredRs);

            var singleRowCtr = new SingleRowConstraint(filter);

            var result = singleRowCtr.ApplyTo(actual);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Status, Is.EqualTo(ConstraintStatus.Failure));
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

            var singleRowCtr = new SingleRowConstraint(filter);

            var result = singleRowCtr.ApplyTo(actual);
            Assert.That(result.ActualValue, Is.Not.Null);
            Assert.That(result.ActualValue, Is.EqualTo(actualRs));
        }

    }
}
