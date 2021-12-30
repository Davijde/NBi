using NBi.Core.ResultSet;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.ResultSetComparison.Parallelization
{
    class ParallelResolverEngine : MultipleResolverEngine
    {
        public ParallelResolverEngine(IResultSetResolver actual, IResultSetResolver expected)
            : base(actual, expected) { }

        public override void Execute()
        {
            Parallel.Invoke(
                () => { Actual = ActualService.Execute(); },
                () => { Expected = ExpectedService.Execute(); }
            );
        }
    }
}
