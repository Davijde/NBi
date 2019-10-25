using NBi.Core.Configuration;
using NBi.Core.ResultSet;
using NBi.Framework.FailureMessage;
using NUnit.Framework.Constraints;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class RowCountConstraintResult : ConstraintResult
    {
        protected IConfiguration Configuration { get; }
        protected IDataRowsMessageFormatter Failure { get; }
        protected ConstraintResult ChildResult { get; }

        public RowCountConstraintResult(RowCountConstraint constraint, ResultSet actual, ConstraintResult childResult)
            : base(constraint, actual, childResult.Status)
        {
            Configuration = constraint.Configuration;
            ChildResult = childResult;
            Failure = BuildFailure(actual, constraint.Configuration);
        }

        protected virtual IDataRowsMessageFormatter BuildFailure(ResultSet actual, IConfiguration configuration)
        {
            var factory = new DataRowsMessageFormatterFactory();
            var msg = factory.Instantiate(configuration.FailureReportProfile, EngineStyle.ByIndex);
            msg.BuildCount(actual.Rows.Cast<DataRow>());
            return msg;
        }

        public override void WriteMessageTo(MessageWriter writer)
            => WriteMessageTo(writer, "count of rows returned by system-under-test is");

        protected void WriteMessageTo(MessageWriter writer, string text)
        {
            writer.Write($"count of rows returned by system-under-test is");
            ChildResult.WriteMessageTo(writer);
            writer.WriteLine();
            writer.WriteLine("Actual result-set:");
            writer.WriteLine(Failure.RenderActual());
        }
    }
}
