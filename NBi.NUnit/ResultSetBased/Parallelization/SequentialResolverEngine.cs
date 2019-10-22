using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.ResultSetComparison.Parallelization
{
    class SequentialResolverEngine : MultipleResolverEngine
    {
        public SequentialResolverEngine(IResultSetService actual, IResultSetService expected)
            : base(actual, expected) { }

        public override void Execute()
        {
            Actual = ActualService.Execute();
            Expected = ExpectedService.Execute();
        }
    }
}
