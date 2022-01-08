using System;
using System.Linq;
using NUnit.Framework.Constraints;
using NBi.Core.ResultSet.Filtering;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class SomeRowsConstraint : RowCountFilterConstraint
    {
        public SomeRowsConstraint(IPredicateFilter filter)
            : base(new GreaterThanOrEqualConstraint(1), filter)
        { }
    }
}