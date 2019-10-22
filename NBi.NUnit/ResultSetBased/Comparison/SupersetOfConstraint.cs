using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet;
using NBi.Extensibility.Resolving;
using NBi.Core.ResultSet.Equivalence;
using NUnit.Framework.Constraints;

namespace NBi.NUnit.ResultSetBased.Comparison
{
    class SupersetOfConstraint : BaseResultSetComparisonConstraint
    {
        public SupersetOfConstraint(IResultSetResolver service)
            : base(service)
        { }

        public new SupersetOfConstraint Using(IEquivaler engine)
            => base.Using(engine) as SupersetOfConstraint;
    }
}
