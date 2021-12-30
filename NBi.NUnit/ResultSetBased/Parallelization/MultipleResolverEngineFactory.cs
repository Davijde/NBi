using NBi.Core.ResultSet;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.ResultSetComparison.Parallelization
{
    public class MultipleResolverEngineFactory
    {
        public MultipleResolverEngine Instantiate(IResultSetResolver actual, IResultSetResolver expected, bool isParallel)
        {
            if (isParallel)
                return new ParallelResolverEngine(actual, expected);
            else
                return new SequentialResolverEngine(actual, expected);
        }
    }
}
