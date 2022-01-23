using NBi.Core.ResultSet;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.ResultSetComparison.Parallelization
{
    class SequentialResolverEngine : MultipleResolverEngine
    {
        public SequentialResolverEngine(IResultSetResolver actual, IResultSetResolver expected)
            : base(actual, expected) { }

        public override (IResultSet, IResultSet) Execute()
        {
            Actual = ActualResolver.Execute();
            Expected = ExpectedResolver.Execute();
            return (Actual, Expected);  
        }
    }
}
