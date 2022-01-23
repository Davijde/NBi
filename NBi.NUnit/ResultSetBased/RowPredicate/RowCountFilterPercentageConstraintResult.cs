using System;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet;
using NBi.Core.Calculation;
using NBi.NUnit.Messaging;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.NUnit.Messaging.Markdown;
using NBi.Framework;
using NBi.Core.Configuration.FailureReport;
using NUnit.Framework.Constraints;
using NBi.Extensibility;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class RowCountFilterPercentageConstraintResult : RowCountFilterConstraintResult
    {
        public new RowCountFilterPercentageConstraint Constraint
        { get => (RowCountFilterPercentageConstraint)base.Constraint; }

        public RowCountFilterPercentageConstraintResult(RowCountFilterPercentageConstraint constraint, IResultSet actual, IResultSet filtered, ConstraintResult childResult)
            : base(constraint, actual, filtered, childResult) { }

        public override void WriteMessageTo(MessageWriter writer)
        {
            var factory = new RowCountFilteredPercentageMessengerFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile, Style);   
            var value = msg.WriteMessage(ActualValue, FilteredValue, ChildResult);
            writer.Write(value);
        }

        public decimal Threshold
        {
            get
            {
                (_, var threshold) = ((RowCountFilterPercentageConstraint)base.Constraint).Describe();
                return threshold;
            }
        }
    }
}