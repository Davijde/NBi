using NBi.Core;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.NUnit.Messaging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Extensibility.Resolving;
using NBi.Extensibility;
using NBi.Core.ResultSet.Lookup.Violation;
using NUnit.Framework.Constraints;
using NBi.NUnit.ResultSetComparison.Parallelization;

namespace NBi.NUnit.ResultSetBased.Lookup
{
    public class ReverseExistsConstraint : ExistsConstraint
    {
        public ReverseExistsConstraint(IResultSetResolver reference)
            : base(reference)
        { }

        public new ReverseExistsConstraint Using(ColumnMappingCollection mappings)
            => base.Using(mappings) as ReverseExistsConstraint;

        internal new ReverseExistsConstraint Using(LookupExistsAnalyzer analyzer)
        {
            Engine = analyzer;
            return this;
        }

        protected override MultipleResolverEngine SpecifyResolvers(IResultSetResolver actual, IResultSetResolver expected)
            => new MultipleResolverEngineFactory().Instantiate(expected, actual, ParallelizeQueries);

        protected override ConstraintResult BuildResult(LookupViolationCollection violations, ConstraintResult childResult)
            => new ReverseExistsConstraintResult(this, Candidate, Reference, violations, KeyMappings, childResult);
    }
}
