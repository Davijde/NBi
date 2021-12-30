using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Extensibility.Resolving;
using NBi.Core.ResultSet.Equivalence;
using NBi.Core.ResultSet.Analyzer;

namespace NBi.NUnit.ResultSetBased.Comparison
{
    public class IntersectionOfConstraint : BaseResultSetComparisonConstraint
    {
        public IntersectionOfConstraint(IResultSetResolver resolver)
            : base(resolver, new AnalyzersFactory().Instantiate(EquivalenceKind.IntersectionOf))
        { }

        public new IntersectionOfConstraint Using(IEquivaler engine)
            => base.Using(engine) as IntersectionOfConstraint;
    }
}
