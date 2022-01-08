using System;
using System.Linq;
using NUnit.Framework.Constraints;
using NBi.Extensibility.Resolving;
using NBi.Extensibility;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class RowCountConstraint : NBiConstraint
    {
        protected IResultSet Actual { get; }
        protected internal Constraint ChildConstraint { get; }

        public RowCountConstraint(Constraint childConstraint)
            => ChildConstraint = childConstraint;

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            switch (actual)
            {
                case IResultSetResolver x: return Matches(x);
                default: throw new ArgumentException();
            }
        }

        protected virtual ConstraintResult Matches(IResultSetResolver actual)
        {
            var actualRs = actual.Execute();
            var childResult = Matches(actualRs);
            return new RowCountConstraintResult(this, actualRs, childResult);
        }

        protected ConstraintResult Matches(IResultSet effective)
            => ChildConstraint.ApplyTo(effective.RowCount);
    }
}