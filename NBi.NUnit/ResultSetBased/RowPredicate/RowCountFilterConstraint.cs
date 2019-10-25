using System;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet;
using NBi.Core.Calculation;
using NBi.Framework.FailureMessage;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework.FailureMessage.Markdown;
using NBi.Framework;
using NBi.Core.Configuration.FailureReport;
using NUnit.Framework.Constraints;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class RowCountFilterConstraint : RowCountConstraint
    {
        protected IResultSetFilter Filter { get; }
        protected Func<ResultSet, ResultSet> FilterFunction { get; }

        public RowCountFilterConstraint(NUnitCtr.Constraint childConstraint, IResultSetFilter filter)
            : this(childConstraint, filter, filter.Apply) { }; 

        protected RowCountFilterConstraint(NUnitCtr.Constraint childConstraint, IResultSetFilter filter, Func<ResultSet, ResultSet> filterFunction)
            : base (childConstraint)
            => (Filter, FilterFunction) = (filter, filterFunction);

        protected override ConstraintResult Matches(IResultSetService actual)
        {
            var filterResultSet = FilterFunction(actual.Execute());
            var childResult = Matches(filterResultSet);
            return new RowCountFilterConstraintResult(this, filterResultSet, childResult);
        }


    }
}