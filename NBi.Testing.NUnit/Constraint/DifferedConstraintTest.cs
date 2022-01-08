using Moq;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility.Resolving;
using NBi.NUnit;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.NUnit.Constraint
{
    public class DifferedConstraintTest
    {

        [Test]
        public void ApplyTo_ScalarResolver_CallToExecuteOnce()
        {
            var expected = Mock.Of<IScalarResolver<decimal>>();
            Mock.Get(expected).Setup(e => e.Execute()).Returns(4);

            var differed = new DifferedConstraint(typeof(GreaterThanConstraint), expected);
            Assert.That(5, differed);

            Mock.Get(expected).Verify(e => e.Execute(), Times.Once);
        }

        [Test]
        public void ApplyTo_GreaterThanInt32Success_Pass()
        {
            var differed = new DifferedConstraint(typeof(GreaterThanConstraint), new LiteralScalarResolver<decimal>(new LiteralScalarResolverArgs(3)));
            Assert.That(5, differed);
        }

        [Test]
        public void ApplyTo_GreaterThanInt32Fail_ThrowException()
        {
            var differed = new DifferedConstraint(typeof(GreaterThanConstraint), new LiteralScalarResolver<decimal>(new LiteralScalarResolverArgs(3)));
            Assert.Throws<AssertionException>(() => Assert.That(2, differed));
        }
    }
}
