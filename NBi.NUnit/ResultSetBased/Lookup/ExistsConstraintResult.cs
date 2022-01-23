using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Equivalence;
using NBi.Core.ResultSet.Lookup;
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
    internal class ExistsConstraintResult : BaseLookupConstraintResult
    {
        protected ColumnMappingCollection KeyMappings { get; }

        public ExistsConstraintResult(ExistsConstraint constraint, IResultSet actual, IResultSet expected, LookupViolationCollection violations, ColumnMappingCollection keyMappings, ConstraintResult result)
            : base(constraint, actual, expected, violations, result)
        => KeyMappings = keyMappings;

        protected override ILookupViolationMessageFormatter BuildFailure()
        {
            var factory = new LookupExistsViolationsMessageFormatterFactory();
            var writer = factory.Instantiate(Configuration.FailureReportProfile);
            writer.Generate(ExpectedValue.Rows, ActualValue.Rows, Violations, KeyMappings, null);
            return writer;
        }
    }
}
