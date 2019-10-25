using System;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet;
using NBi.Framework.FailureMessage;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework.FailureMessage.Markdown;
using NUnit.Framework;
using NBi.Framework;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Configuration.FailureReport;
using NUnit.Framework.Constraints;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class RowCountConstraint : NBiConstraint
    {
        protected IResultSetService Actual { get; }
        protected internal NUnitCtr.Constraint childConstraint { get; }

        public RowCountConstraint(NUnitCtr.Constraint childConstraint)
            => this.childConstraint = childConstraint;

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            switch (actual)
            {
                case IResultSetService x: return Matches(x);
                default: throw new ArgumentException();
            }
        }

        protected virtual ConstraintResult Matches(IResultSetService actual)
        {
            var actualRs = actual.Execute();
            var childResult = Matches(actualRs);
            return new RowCountConstraintResult(this, actualRs, childResult);
        }

        protected ConstraintResult Matches(ResultSet effective)
            => childConstraint.ApplyTo(effective.Rows.Count);
    }
}