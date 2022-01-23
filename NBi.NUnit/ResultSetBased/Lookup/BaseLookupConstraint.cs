using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using NBi.Extensibility.Resolving;
using NBi.Core.ResultSet.Lookup;
using NBi.NUnit.ResultSetComparison.Parallelization;
using NBi.Extensibility;
using NBi.Core.ResultSet.Lookup.Violation;

namespace NBi.NUnit.ResultSetBased.Lookup
{
    public abstract class BaseLookupConstraint : NBiConstraint
    {
        protected IResultSetResolver Expected { get; set; }

        protected IResultSet Reference { get; set; }
        protected IResultSet Candidate { get; set; }

        private Constraint ChildConstraint { get; } = new EmptyCollectionConstraint();

        public ILookupAnalyzer Engine { get; protected set; }

        protected bool ParallelizeQueries { get; private set; } = false;

        protected BaseLookupConstraint(IResultSetResolver resolver, ILookupAnalyzer engine)
            => (Expected, Engine) = (resolver, engine);

        public abstract BaseLookupConstraint Using(ColumnMappingCollection mappings);

        public BaseLookupConstraint WithParallelQueries()
        {
            ParallelizeQueries = true;
            return this;
        }

        public BaseLookupConstraint WithSequentialQueries()
        {
            ParallelizeQueries = false;
            return this;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            switch (actual)
            {
                case IResultSetResolver x: return Matches(x);
                default: throw new ArgumentException();
            }
        }

        protected ConstraintResult Matches(IResultSetResolver actual)
        {
            var resolvers = SpecifyResolvers(actual, Expected);
            (Candidate, Reference) = resolvers.Execute();

            var violations = Engine.Execute(Candidate, Reference);
            var childResult = ChildConstraint.ApplyTo(violations);

            return BuildResult(violations, childResult);
        }

        protected virtual MultipleResolverEngine SpecifyResolvers(IResultSetResolver actual, IResultSetResolver expected)
            => new MultipleResolverEngineFactory().Instantiate(actual, expected, ParallelizeQueries);

        protected abstract ConstraintResult BuildResult(LookupViolationCollection violations, ConstraintResult childResult);
    }
}