using System;
using System.Linq;
using NUnit.Framework.Constraints;
using NBi.Core.ResultSet.Filtering;
using NBi.Extensibility;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class RowCountFilterConstraint : RowCountConstraint
    {
        protected IResultSetFilter Filter { get; }
        protected Func<IResultSet, IResultSet> FilterFunction { get; }

        public RowCountFilterConstraint(Constraint childConstraint, IResultSetFilter filter)
            : this(childConstraint, filter, filter.Apply) { } 

        protected RowCountFilterConstraint(Constraint childConstraint, IResultSetFilter filter, Func<IResultSet, IResultSet> filterFunction)
            : base (childConstraint)
            => (Filter, FilterFunction) = (filter, filterFunction);

        //protected override ConstraintResult Matches(IResultSetService actual)
        //{
        //    var filterResultSet = FilterFunction(actual.Execute());
        //    var childResult = Matches(filterResultSet);
        //    return new RowCountFilterConstraintResult(this, filterResultSet, childResult);
        //}
    }
}