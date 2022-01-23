using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Extensibility;
using NBi.NUnit.Messaging;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.ResultSetBased.Lookup
{
    internal class MatchesConstraintResult : ExistsConstraintResult
    {
        protected ColumnMappingCollection ValueMappings { get; }

        public MatchesConstraintResult(ExistsConstraint constraint, IResultSet actual, IResultSet expected, LookupViolationCollection violations, ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings, ConstraintResult result)
            : base(constraint, actual, expected, violations, keyMappings, result)
        => ValueMappings = valueMappings;

        protected override ILookupViolationMessageFormatter BuildFailure()
        {
            var factory = new LookupMatchesViolationsMessageFormatterFactory();
            var writer = factory.Instantiate(Configuration.FailureReportProfile);
            writer.Generate(ExpectedValue.Rows, ActualValue.Rows, Violations, KeyMappings, ValueMappings);
            return writer;
        }
    }
}
