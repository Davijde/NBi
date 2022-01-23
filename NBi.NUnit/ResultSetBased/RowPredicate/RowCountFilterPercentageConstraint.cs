using System;
using System.Linq;
using NUnit.Framework.Constraints;
using NBi.Core.ResultSet.Filtering;
using NBi.Extensibility.Resolving;
using static System.FormattableString;
using System.Globalization;

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
            return new RowCountFilterPercentageConstraintResult(this, actualRs, filteredRs, childResult);
        }

        public override string Description
        {
            get
            {
                (var description, var percentage) = Describe();
                return Invariant($"{description} {percentage:F2}% of rows validating the predicate '{Filter.Describe()}'");
            }
            protected set => base.Description = value;
        }

        internal (string, decimal) Describe()
        {
            var lastToken = ChildConstraint.Description.Split(' ').Reverse().ElementAt(0);
            var percentage = decimal.Parse(lastToken.Trim('d'), CultureInfo.InvariantCulture.NumberFormat) * 100;
            var description = ChildConstraint.Description.Replace(lastToken, "").TrimEnd();
            return (description, percentage);
        }
    }
}