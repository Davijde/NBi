using NBi.Core.Configuration;
using NBi.Core.Variable;
using NBi.Extensibility;
using NBi.Framework;
using NBi.NUnit.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public abstract class ResultSetBasedConstraintResult : NBiConstraintResult
    {
        public new IResultSet ActualValue { get => (IResultSet)base.ActualValue; }

        public ResultSetBasedConstraintResult(NBiConstraint constraint, object actualValue, bool isSuccess)
        : base(constraint, actualValue, isSuccess) { }
            
    }
}
