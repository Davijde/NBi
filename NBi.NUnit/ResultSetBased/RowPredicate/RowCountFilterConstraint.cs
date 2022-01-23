using System;
using System.Linq;
using NUnit.Framework.Constraints;
using NBi.Core.ResultSet.Filtering;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class RowCountFilterConstraint : RowCountConstraint
    {
        protected IResultSetFilter Filter { get; }

        public RowCountFilterConstraint(Constraint childConstraint, IResultSetFilter filter)
            : base(childConstraint) => Filter = filter;

        protected override ConstraintResult Matches(IResultSetResolver actual)
        {
            var actualRs = actual.Execute();
            var filterResultSet = Filter.Execute(actualRs);
            var childResult = Matches(filterResultSet);
            return new RowCountFilterConstraintResult(this, actualRs, filterResultSet, childResult);
        }

        public override string Description
        {
            get => $"{ChildConstraint.Description} rows validating the predicate '{Filter.Describe()}'";
            protected set => base.Description = value;
        }
    }
}