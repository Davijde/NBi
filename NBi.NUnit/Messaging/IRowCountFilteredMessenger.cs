using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Discrimination;
using NBi.Core.ResultSet.Uniqueness;
using NBi.Extensibility;
using NBi.NUnit.ResultSetBased.RowPredicate;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Messaging
{
    public interface IRowCountFilteredMessenger
    {
        string WriteMessage(IResultSet actual, IResultSet filtered, ConstraintResult result);
    }
}
