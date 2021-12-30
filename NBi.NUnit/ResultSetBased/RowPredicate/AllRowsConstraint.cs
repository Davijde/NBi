using System;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet.Filtering;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class AllRowsConstraint : NoRowsConstraint
    {
        public AllRowsConstraint(IResultSetFilter filter)
            : base(filter, filter.AntiApply) { }
    }
}