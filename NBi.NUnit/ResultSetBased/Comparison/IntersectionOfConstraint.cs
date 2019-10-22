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
    public class IntersectionOfConstraint : BaseResultSetComparisonConstraint
    {
        public IntersectionOfConstraint(IResultSetResolver resolver)
            : base(resolver)
        { }

        public new IntersectionOfConstraint Using(IEquivaler engine)
            => base.Using(engine) as IntersectionOfConstraint;
    }
}
