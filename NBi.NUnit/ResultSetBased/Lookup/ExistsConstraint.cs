using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using NBi.NUnit.Messaging;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.ResultSetBased.Lookup
{
    public class ExistsConstraint : BaseLookupConstraint
    {
        protected ColumnMappingCollection KeyMappings { get; set; }

        public ExistsConstraint(IResultSetResolver reference)
            : base(reference, new LookupExistsAnalyzer())
            { }

        public override BaseLookupConstraint Using(ColumnMappingCollection mappings)
        {
            KeyMappings = mappings;
            Engine = (Engine as LookupExistsAnalyzer).UsingKeys(mappings);
            return this;
        }

        internal ExistsConstraint Using(LookupExistsAnalyzer analyzer)
        {
            Engine = analyzer;
            return this;
        }

        protected override ConstraintResult BuildResult(LookupViolationCollection violations, ConstraintResult childResult)
            => new ExistsConstraintResult(this, Candidate, Reference, violations, KeyMappings, childResult);
    }
}
