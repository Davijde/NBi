using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core.ResultSet;
using NBi.Extensibility.Resolving;
using NBi.Core.ResultSet.Equivalence;

namespace NBi.NUnit.ResultSetBased.Comparison
{
    public class SubsetOfConstraint : BaseResultSetComparisonConstraint
    {
        public SubsetOfConstraint(IResultSetResolver service)
            : base(service)
        { }


        public new SubsetOfConstraint Using(IEquivaler engine)
            => base.Using(engine) as SubsetOfConstraint;
    }
}
