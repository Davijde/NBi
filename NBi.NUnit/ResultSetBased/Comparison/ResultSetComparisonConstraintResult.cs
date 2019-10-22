using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Equivalence;
using NBi.Framework.FailureMessage;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.ResultSetBased.Comparison
{
    internal class ResultSetComparisonConstraintResult : ConstraintResult
    {
        protected IConfiguration Configuration { get; }
        protected IDataRowsMessageFormatter Failure { get; }

        public ResultSetComparisonConstraintResult(BaseResultSetComparisonConstraint constraint, ResultSet actual, ResultSet expected, ResultResultSet result)
            : base(constraint, actual, result.Difference == ResultSetDifferenceType.None)
        { 
            Configuration = constraint.Configuration;
            Failure = BuildFailure(actual, expected, result, constraint.Engine, constraint.Configuration);
        }

        protected virtual IDataRowsMessageFormatter BuildFailure(ResultSet actual, ResultSet expected, ResultResultSet result, IEquivaler engine, IConfiguration configuration)
        {
            var factory = new DataRowsMessageFormatterFactory();
            var msg = factory.Instantiate(configuration.FailureReportProfile, engine.Style);
            msg.BuildComparaison(expected.Rows.Cast<DataRow>(), actual.Rows.Cast<DataRow>(), result);
            return msg;
        }

        public override void WriteMessageTo(MessageWriter writer)
        {
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                writer.Write(Failure.RenderMessage());
            else
            {
                writer.WriteLine("Execution of the query doesn't match the expected result ");
                writer.WriteLine();
                writer.WriteLine("  Expected: ");
                writer.WriteLine(Failure.RenderExpected());
                writer.WriteLine();
                writer.WriteLine("  But was:  ");
                writer.WriteLine(Failure.RenderActual());
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine(Failure.RenderAnalysis());
            }
        }
    }
}
