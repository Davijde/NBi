using System;
using System.Linq;
using NUnit.Framework.Constraints;
using NBi.Core.ResultSet.Filtering;
using NBi.Extensibility.Resolving;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class RowCountFilterPercentageConstraint : RowCountFilterConstraint
    {
        public RowCountFilterPercentageConstraint(Constraint childConstraint, IResultSetFilter filter)
            : base(childConstraint, filter)
            { } 

        protected override ConstraintResult Matches(IResultSetResolver actual)
        {
            var actualRs = actual.Execute();
            var filteredRs = Filter.Execute(actualRs);
            decimal effectivePercentage = filteredRs.RowCount / Convert.ToDecimal(actualRs.RowCount);
            var childResult = ChildConstraint.ApplyTo(effectivePercentage);
            return new RowCountFilterPercentageConstraintResult(this, actualRs, childResult);
        }
    }
}