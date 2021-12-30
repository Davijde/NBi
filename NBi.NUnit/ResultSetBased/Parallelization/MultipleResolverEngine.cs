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
        protected IResultSetResolver ActualService { get; }
        protected IResultSetResolver ExpectedService { get; }

        public IResultSet Actual { get; protected set; }
        public IResultSet Expected { get; protected set; }

        public MultipleResolverEngine(IResultSetResolver actual, IResultSetResolver expected)
            => (ActualService, ExpectedService) = (actual, expected);

        public abstract void Execute();
    }
}
