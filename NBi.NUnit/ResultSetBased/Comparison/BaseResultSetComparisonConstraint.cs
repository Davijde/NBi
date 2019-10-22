using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core.ResultSet;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework.FailureMessage;
using NBi.Core.ResultSet.Analyzer;
using NBi.Core.ResultSet.Equivalence;
using NUnit.Framework;
using NBi.Core.Configuration.FailureReport;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using NUnit.Framework.Constraints;
using NBi.NUnit.ResultSetComparison.Parallelization;

namespace NBi.NUnit.ResultSetBased.Comparison
{
    public abstract class BaseResultSetComparisonConstraint : NBiConstraint
    {
        protected IResultSetService Expected { get; }
        protected IResultSetService Actual { get; }
        public IEquivaler Engine { get; private set; } = new OrdinalEquivaler(AnalyzersFactory.EqualTo(), null);

        private bool parallelizeQueries = false;

        public BaseResultSetComparisonConstraint(IResultSetService expected)
            => (Expected) = (expected);

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
            this.parallelizeQueries = true;
            return this;
        }

        public BaseResultSetComparisonConstraint WithSequentialQueries()
        {
            this.parallelizeQueries = false;
            return this;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            switch (actual)
            {
                case IResultSetService x: return Matches(x);
                default: throw new ArgumentException();
            }
        }

        protected ConstraintResult Matches(IResultSetService actual)
        {
            var resolvers = new MultipleResolverEngineFactory().Instantiate(actual, Expected, parallelizeQueries);
            resolvers.Execute();

            var result = Engine.Compare(resolvers.Actual, resolvers.Expected);
            return new ResultSetComparisonConstraintResult(this, resolvers.Actual, resolvers.Expected, result);
        }

        internal bool IsParallelizeQueries()
        {
            return parallelizeQueries;
        }
    }
}
