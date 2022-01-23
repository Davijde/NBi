using System;
using System.Linq;
using System.Collections.Generic;
using Moq;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using NBi.NUnit.ResultSetBased.Lookup;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace NBi.Testing.NUnit.Constraint.ResultSetBased.Comparison
{
    [TestFixture]
    public class LookupReverseExistsConstraintTest
    {
        [Test]
        public void Matches_ActualResultSetResolver_ExecuteCalledOnce()
        {
            var reference = Mock.Of<IResultSet>();
            var expected = Mock.Of<IResultSetResolver>();
            Mock.Get(expected).Setup(s => s.Execute()).Returns(reference);

            var candidate = Mock.Of<IResultSet>();
            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(candidate);

            var analyzer = Mock.Of<LookupExistsAnalyzer>();
            Mock.Get(analyzer).Setup(a => a.Execute(It.IsAny<IResultSet>(), It.IsAny<IResultSet>())).Returns(LookupExistsViolationCollection.Empty);

            var reverseCtr = new ReverseExistsConstraint(expected);
            reverseCtr = reverseCtr.Using(analyzer);

            reverseCtr.ApplyTo(actual);

            Mock.Get(actual).Verify(s => s.Execute(), Times.Once);
         }

        [Test]
        public void Matches_ExpectedResultSetResolver_ExecuteCalledOnce()
        {
            var reference = Mock.Of<IResultSet>();
            var expected = Mock.Of<IResultSetResolver>();
            Mock.Get(expected).Setup(s => s.Execute()).Returns(reference);

            var candidate = Mock.Of<IResultSet>();
            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(candidate);

            var analyzer = Mock.Of<LookupExistsAnalyzer>();
            Mock.Get(analyzer).Setup(a => a.Execute(It.IsAny<IResultSet>(), It.IsAny<IResultSet>())).Returns(LookupExistsViolationCollection.Empty);

            var reverseCtr = new ReverseExistsConstraint(expected);
            reverseCtr = reverseCtr.Using(analyzer);

            reverseCtr.ApplyTo(actual);

            Mock.Get(expected).Verify(s => s.Execute(), Times.Once);
        }

        [Test]
        public void Matches_Analyzer_ExecuteCalledOnce()
        {
            var candidate = Mock.Of<IResultSet>();
            var expected = Mock.Of<IResultSetResolver>();
            Mock.Get(expected).Setup(s => s.Execute()).Returns(candidate);

            var reference = Mock.Of<IResultSet>();
            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(reference);

            var analyzer = Mock.Of<LookupExistsAnalyzer>();
            Mock.Get(analyzer).Setup(a => a.Execute(It.IsAny<IResultSet>(), It.IsAny<IResultSet>())).Returns(LookupExistsViolationCollection.Empty);

            var reverseCtr = new ReverseExistsConstraint(expected);
            reverseCtr = reverseCtr.Using(analyzer);

            reverseCtr.ApplyTo(actual);

            Mock.Get(analyzer).Verify(s => s.Execute(
                It.Is<IResultSet>(x => x == candidate)
                , It.Is<IResultSet>(y => y == reference))
                , Times.Once);
        }

        [Test]
        public void ApplyTo_ResultZeroRow_Success()
        {
            var reference = Mock.Of<IResultSet>();
            var expected = Mock.Of<IResultSetResolver>();
            Mock.Get(expected).Setup(s => s.Execute()).Returns(reference);

            var candidate = Mock.Of<IResultSet>();
            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(candidate);

            var analyzer = Mock.Of<LookupExistsAnalyzer>();
            Mock.Get(analyzer).Setup(a => a.Execute(It.IsAny<IResultSet>(), It.IsAny<IResultSet>())).Returns(LookupExistsViolationCollection.Empty);

            var reverseCtr = new ReverseExistsConstraint(expected);
            reverseCtr = reverseCtr.Using(analyzer);

            var result = reverseCtr.ApplyTo(actual);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Status, Is.EqualTo(ConstraintStatus.Success));
        }

        [Test]
        public void ApplyTo_ResultOneRow_Failure()
        {
            var reference = Mock.Of<IResultSet>();
            var expected = Mock.Of<IResultSetResolver>();
            Mock.Get(expected).Setup(s => s.Execute()).Returns(reference);

            var candidate = Mock.Of<IResultSet>();
            var actual = Mock.Of<IResultSetResolver>();
            Mock.Get(actual).Setup(s => s.Execute()).Returns(candidate);

            var violations = new LookupExistsViolationCollection(ColumnMappingCollection.DefaultKey);
            violations.Add(new KeyCollection(new[] { "foo" }), new LookupExistsViolationInformation(RowViolationState.Missing));

            var analyzer = Mock.Of<LookupExistsAnalyzer>();
            Mock.Get(analyzer).Setup(a => a.Execute(It.IsAny<IResultSet>(), It.IsAny<IResultSet>())).Returns(violations);

            var reverseCtr = new ReverseExistsConstraint(expected);
            reverseCtr = reverseCtr.Using(analyzer);

            var result = reverseCtr.ApplyTo(actual);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Status, Is.EqualTo(ConstraintStatus.Failure));
        }
    }
}
