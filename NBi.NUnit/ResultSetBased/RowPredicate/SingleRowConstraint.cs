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

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class SingleRowConstraint : RowCountFilterConstraint
    {
        public SingleRowConstraint(IPredicateFilter filter)
            : base(new NUnitCtr.EqualConstraint(1), filter)
        { }
    }
}