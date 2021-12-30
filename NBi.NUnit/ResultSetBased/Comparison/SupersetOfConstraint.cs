using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NBi.Extensibility.Resolving;
using NBi.Core.ResultSet.Equivalence;
using NUnit.Framework.Constraints;
using NBi.Core.ResultSet.Analyzer;

namespace NBi.NUnit.ResultSetBased.Comparison
{
    class SupersetOfConstraint : BaseResultSetComparisonConstraint
    {
        public SupersetOfConstraint(IResultSetResolver service)
            : base(service, new AnalyzersFactory().Instantiate(EquivalenceKind.SupersetOf))
        { }

        public new SupersetOfConstraint Using(IEquivaler engine)
            => base.Using(engine) as SupersetOfConstraint;
    }
}
