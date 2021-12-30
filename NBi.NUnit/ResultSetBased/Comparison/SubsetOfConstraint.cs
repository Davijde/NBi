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
    public class SubsetOfConstraint : BaseResultSetComparisonConstraint
    {
        public SubsetOfConstraint(IResultSetResolver service)
            : base(service, new AnalyzersFactory().Instantiate(EquivalenceKind.SubsetOf))
        { }

        public new SubsetOfConstraint Using(IEquivaler engine)
            => base.Using(engine) as SubsetOfConstraint;
    }
}
