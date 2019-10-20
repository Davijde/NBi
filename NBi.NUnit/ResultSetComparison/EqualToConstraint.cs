using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core.ResultSet;
using NBi.Extensibility.Resolving;
using NBi.Core.ResultSet.Equivalence;

namespace NBi.NUnit.ResultSetComparison
{
    public class EqualToConstraint : BaseResultSetComparisonConstraint
    {
        public EqualToConstraint(IResultSetResolver resolver)
            : base(resolver)
        { }

        public new EqualToConstraint Using(IEquivaler engine)
            => base.Using(engine) as EqualToConstraint;
    }
}
