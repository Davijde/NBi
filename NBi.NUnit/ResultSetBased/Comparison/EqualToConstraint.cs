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
    public class EqualToConstraint : BaseResultSetComparisonConstraint
    {
        public EqualToConstraint(IResultSetResolver resolver)
            : base(resolver, new AnalyzersFactory().Instantiate(EquivalenceKind.EqualTo))
        { }

        public new EqualToConstraint Using(IEquivaler engine)
            => base.Using(engine) as EqualToConstraint;
    }
}
