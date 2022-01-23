using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NBi.NUnit.ResultSetComparison.Parallelization
{
    public abstract class MultipleResolverEngine
    {
        protected IResultSetResolver ActualResolver { get; }
        protected IResultSetResolver ExpectedResolver { get; }

        public IResultSet Actual { get; protected set; }
        public IResultSet Expected { get; protected set; }

        public MultipleResolverEngine(IResultSetResolver actual, IResultSetResolver expected)
            => (ActualResolver, ExpectedResolver) = (actual, expected);

        public abstract (IResultSet, IResultSet) Execute();
    }
}
