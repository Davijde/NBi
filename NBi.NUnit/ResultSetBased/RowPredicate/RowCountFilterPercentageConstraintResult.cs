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
        public RowCountFilterPercentageConstraintResult(RowCountConstraint constraint, IResultSet actual, IResultSet filtered, ConstraintResult childResult)
            : base(constraint, actual, filtered, childResult) { }

        //public override void WriteMessageTo(MessageWriter writer)
        //    => WriteMessageTo(writer, $"percentage of rows matching the predicate is {TransformDecimalToPercentage(this.WriteDescriptionTo)}");

        //protected string TransformDecimalToPercentage(Action<NUnitFwk.TextMessageWriter> action)
        //{
        //    Constraint ctr;
        //    ctr.

        //    var sb = new System.Text.StringBuilder();
        //    var localWriter = new NUnitFwk.TextMessageWriter();
        //    action(localWriter);
        //    var childMessage = localWriter.ToString();
        //    sb.Append(childMessage.Substring(0, childMessage.LastIndexOf(" ") + 1));
        //    sb.Append(decimal.Parse(childMessage.Substring(childMessage.LastIndexOf(" ") + 1).Replace("m", ""), System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat));
        //    sb.Append("%");

        //    return sb.ToString();
        //}
    }
}