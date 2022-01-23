using NBi.Core.Configuration;
using NBi.Core.ResultSet;
using NBi.Extensibility;
using NBi.NUnit.Messaging;
using NUnit.Framework.Constraints;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class RowCountConstraintResult : ResultSetBasedConstraintResult
    {
        public EngineStyle Style { get; } = EngineStyle.ByIndex;
        public ConstraintResult ChildResult { get; }

        public RowCountConstraintResult(RowCountConstraint constraint, IResultSet actual, ConstraintResult childResult)
            : base(constraint, actual, childResult.IsSuccess)
            => (ChildResult, Style) = (childResult, EngineStyle.ByIndex);

        public override void WriteMessageTo(MessageWriter writer)
        {
            var factory = new RowCountMessengerFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile, Style);
            var value = msg.WriteMessage(ActualValue, ChildResult);
            writer.Write(value);
        }
    }
}
