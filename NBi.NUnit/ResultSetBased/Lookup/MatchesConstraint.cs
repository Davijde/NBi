using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.ResultSetBased.Lookup
{
    public class MatchesConstraint : ExistsConstraint
    {
        protected ColumnMappingCollection ValueMappings { get; private set; }

        public MatchesConstraint(IResultSetResolver reference)
            : base(reference)
        { }

        public override BaseLookupConstraint Using(ColumnMappingCollection valueMappings)
        {
            ValueMappings = valueMappings;
            Engine = new LookupMatchesAnalyzer()
                        .UsingKeys(ColumnMappingCollection.DefaultKey)
                        .UsingValues(valueMappings ?? throw new ArgumentNullException())
                        .Using(new Dictionary<IColumnIdentifier, Core.Scalar.Comparer.Tolerance>());
            return this;
        }

        internal MatchesConstraint Using(LookupMatchesAnalyzer analyzer)
        {
            Engine = analyzer;
            return this;
        }

        public BaseLookupConstraint Using(ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings, IDictionary<IColumnIdentifier, Core.Scalar.Comparer.Tolerance> tolerances)
        {
            (KeyMappings, ValueMappings) = (keyMappings, valueMappings);
            Engine = new LookupMatchesAnalyzer()
                        .UsingKeys(keyMappings ?? ColumnMappingCollection.DefaultKey)
                        .UsingValues(valueMappings ?? throw new ArgumentNullException())
                        .Using(tolerances);
            return this;
        }

        protected override ConstraintResult BuildResult(LookupViolationCollection violations, ConstraintResult childResult)
            => new MatchesConstraintResult(this, Candidate, Reference, violations, KeyMappings, ValueMappings, childResult);
    }
}
