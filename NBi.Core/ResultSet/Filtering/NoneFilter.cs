using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering
{
    class NoneFilter : IResultSetFilter
    {
        protected Func<IResultSet, IResultSet> Execution { get; }

        public NoneFilter()
            => Execution = Keep;

        public IResultSet Execute(IResultSet rs)
            => Execution.Invoke(rs);

        public IResultSet Keep(IResultSet rs)
            => rs ?? throw new ArgumentNullException();

        public IResultSet Discard(IResultSet rs)
            => rs ?? throw new ArgumentNullException();

        public string Describe() => "none";
    }
}
