using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework.Constraints;
using NBi.Extensibility;
using NBi.Core.ResultSet.Discrimination;
using NBi.Core.Configuration;
using NBi.NUnit.Messaging;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;

namespace NBi.NUnit.ResultSetBased.Discrimination
{
    public class UniqueRowsConstraintResult : ResultSetBasedConstraintResult
    {
        protected ResultUniqueRows ResultValue { get; }
        public EngineStyle Style { get; } = EngineStyle.ByIndex;

        public UniqueRowsConstraintResult(UniqueRowsConstraint constraint, IResultSet actual, ResultUniqueRows result)
            : base(constraint, actual, result.AreUnique)
            => (ResultValue) = (result);

        public override void WriteMessageTo(MessageWriter writer)
        {
            var factory = new UniquenessMessengerFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile, Style);
            var value = msg.WriteMessage(ActualValue, ResultValue);
            writer.Write(value);
        }
    }
}
