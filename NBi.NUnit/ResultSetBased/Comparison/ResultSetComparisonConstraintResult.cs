using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Equivalence;
using NBi.Extensibility;
using NBi.NUnit.Messaging;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.ResultSetBased.Comparison
{
    internal class ResultSetComparisonConstraintResult : ResultSetBasedConstraintResult
    {
        public IResultSet ExpectedValue { get; }
        public ResultResultSet ResultValue { get; }
        public EngineStyle Style { get; }

        public ResultSetComparisonConstraintResult(BaseResultSetComparisonConstraint constraint, IResultSet actual, IResultSet expected, ResultResultSet result)
            : base(constraint, actual, result.Difference == ResultSetDifferenceType.None)
            => (ExpectedValue, ResultValue, Style) = (expected, result, constraint.Engine.Style);

        public override void WriteMessageTo(MessageWriter writer)
        {
            var factory = new ComparisonMessengerFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile, Style);
            var value = msg.WriteMessage(ExpectedValue, ActualValue, ResultValue);
            writer.Write(value);
        }
    }
}
