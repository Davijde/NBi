using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Resolving;
using NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public class DifferedConstraint : Constraint
    {
        private Type ConstraintType { get; }
        private IScalarResolver<decimal> Resolver { get; }

        public DifferedConstraint(Type constraintType, IScalarResolver<decimal> resolver)
        {
            ConstraintType = constraintType;
            Resolver = resolver;
        }

        public Constraint Resolve()
        {
            var expected = Resolver.Execute();
            var ctr = (Constraint)Activator.CreateInstance(ConstraintType, expected);
            return ctr;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
            => Resolve().ApplyTo(actual);
    }
}
