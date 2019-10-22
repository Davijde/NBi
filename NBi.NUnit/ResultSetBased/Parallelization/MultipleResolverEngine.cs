using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.ResultSetComparison.Parallelization
{
    public abstract class MultipleResolverEngine
    {
        protected IResultSetService ActualService { get; }
        protected IResultSetService ExpectedService { get; }

        public ResultSet Actual { get; protected set; }
        public ResultSet Expected { get; protected set; }

        public MultipleResolverEngine(IResultSetService actual, IResultSetService expected)
            => (ActualService, ExpectedService) = (actual, expected);

        public abstract void Execute();
    }
}
