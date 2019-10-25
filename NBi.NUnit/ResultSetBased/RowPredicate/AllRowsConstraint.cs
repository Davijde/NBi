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

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class AllRowsConstraint : NoRowsConstraint
    {
        public AllRowsConstraint(IResultSetFilter filter)
            : base(filter, filter.AntiApply) { }
    }
}