using System;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Calculation;
using NBi.Framework.FailureMessage;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet.Filtering;
using NBi.Extensibility;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class NoRowsConstraint : RowCountFilterConstraint
    {
        public NoRowsConstraint(IResultSetFilter filter)
            : this(filter, filter.Apply) { }

        protected NoRowsConstraint(IResultSetFilter filter, Func<IResultSet, IResultSet> filterFunction)
            : base(new NUnitCtr.EqualConstraint(0), filter, filterFunction) { }

        protected NoRowsConstraint(NUnitCtr.Constraint childConstraint, IResultSetFilter filter)
            : base(childConstraint, filter, filter.Apply) { }
    }
}