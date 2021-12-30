using System;
using System.Data;
using System.Linq;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.ResultSet;
using NBi.Core.Calculation;
using NBi.Framework.FailureMessage;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework.FailureMessage.Markdown;
using NBi.Framework;
using NBi.Core.Configuration.FailureReport;
using NUnit.Framework.Constraints;
using NBi.Extensibility;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class RowCountFilterConstraintResult : RowCountConstraintResult
    {
        public RowCountFilterConstraintResult(RowCountConstraint constraint, IResultSet filtered, ConstraintResult childResult)
            : base(constraint, filtered, childResult) { }

        public override void WriteMessageTo(MessageWriter writer)
            => WriteMessageTo(writer, "count of rows returned by system-under-test after filtering is");
    }
}