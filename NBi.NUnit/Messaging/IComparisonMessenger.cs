using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Discrimination;
using NBi.Core.ResultSet.Uniqueness;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Messaging
{
    public interface IComparisonMessenger
    {
        string WriteMessage(IResultSet expected, IResultSet actual, ResultResultSet result);
    }
}
