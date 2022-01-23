using NBi.Core.Configuration;
using NBi.Core.Variable;
using NBi.Framework;
using NBi.NUnit.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public abstract class NBiConstraintResult : ConstraintResult
    {
        public IConfiguration Configuration { get; }

        public NBiConstraintResult(NBiConstraint constraint, object actualValue, bool isSuccess)
        : base(constraint, actualValue, isSuccess) 
            => Configuration = constraint.Configuration;
    }
}
