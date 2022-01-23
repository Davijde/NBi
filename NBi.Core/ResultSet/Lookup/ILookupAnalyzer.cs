using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Lookup
{
    public interface ILookupAnalyzer
    {
        LookupViolationCollection Execute(IResultSet candidate, IResultSet reference);
    }
}
