using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core.ResultSet;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.NUnit.Messaging;
using NBi.Core.ResultSet.Analyzer;
using NBi.Core.ResultSet.Equivalence;
using NUnit.Framework;
using NBi.Core.Configuration.FailureReport;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using NUnit.Framework.Constraints;
using NBi.NUnit.ResultSetComparison.Parallelization;
using System.Collections.Generic;

namespace NBi.NUnit.ResultSetBased.Comparison
{
    public abstract class BaseResultSetComparisonConstraint : NBiConstraint
    {
        protected IResultSetResolver Expected { get; }
        protected IResultSetResolver Actual { get; }
        protected IReadOnlyCollection<IRowsAnalyzer> Analyzers { get; }
        public IEquivaler Engine { get; private set; } = new OrdinalEquivaler(null);

        private bool ParallelizeQueries = false;

        protected BaseResultSetComparisonConstraint(IResultSetResolver expected, IEnumerable<IRowsAnalyzer> analyzers)
            => (Expected, Analyzers) = (expected, analyzers.ToList().AsReadOnly());

        public BaseResultSetComparisonConstraint Using(IEquivaler engine)
        {
            Engine = engine;
            return this;
        }

        public BaseResultSetComparisonConstraint Using(ISettingsResultSet settings)
        {
            Engine.Settings = settings;
            return this;
        }

        public BaseResultSetComparisonConstraint WithParallelQueries()
        {
            ParallelizeQueries = true;
            return this;
        }

        public BaseResultSetComparisonConstraint WithSequentialQueries()
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
            var resolvers = new MultipleResolverEngineFactory().Instantiate(actual, Expected, ParallelizeQueries);
            resolvers.Execute();

            var result = Engine.Using(Analyzers).Compare(resolvers.Actual, resolvers.Expected);
            return new ResultSetComparisonConstraintResult(this, resolvers.Actual, resolvers.Expected, result);
        }
    }
}
