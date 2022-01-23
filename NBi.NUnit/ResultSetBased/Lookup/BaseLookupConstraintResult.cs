using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Equivalence;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Extensibility;
using NBi.NUnit.Messaging;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.ResultSetBased.Lookup
{
    public abstract class BaseLookupConstraintResult : ConstraintResult
    {
        protected IConfiguration Configuration { get; }
        protected new IResultSet ActualValue { get => (IResultSet)base.ActualValue; }
        protected IResultSet ExpectedValue { get; }
        protected LookupViolationCollection Violations { get; }


        public BaseLookupConstraintResult(BaseLookupConstraint constraint, IResultSet actual, IResultSet expected, LookupViolationCollection violations, ConstraintResult result)
            : base(constraint, actual, result.Status)
        { 
            Configuration = constraint.Configuration;
            ExpectedValue = expected;
            Violations = violations;
        }

        protected abstract ILookupViolationMessageFormatter BuildFailure();

        public override void WriteMessageTo(MessageWriter writer)
        {
            var failure = BuildFailure();
            if (Configuration.FailureReportProfile.Format == FailureReportFormat.Json)
                writer.Write(failure.RenderMessage());
            else
            {
                writer.WriteLine(failure.RenderPredicate());
                writer.WriteLine();
                writer.WriteLine();
                base.WriteMessageTo(writer);
                writer.WriteLine();
                writer.WriteLine(failure.RenderAnalysis());
            }
        }
    }
}
