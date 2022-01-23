using System;
using System.Data;
using System.Linq;
using NBi.NUnit.Messaging;
using NUnit.Framework.Constraints;
using NBi.Extensibility;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class RowCountFilterConstraintResult : RowCountConstraintResult
    {
        public IResultSet FilteredValue { get; }

        public RowCountFilterConstraintResult(RowCountFilterConstraint constraint, IResultSet actual, IResultSet filtered, ConstraintResult childResult)
            : base(constraint, actual, childResult)
            => FilteredValue = filtered;

        public override void WriteMessageTo(MessageWriter writer)
        {
            var factory = new RowCountFilteredMessengerFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile, Style);
            var value = msg.WriteMessage(ActualValue, FilteredValue, ChildResult);
            writer.Write(value);
        }
    }
}