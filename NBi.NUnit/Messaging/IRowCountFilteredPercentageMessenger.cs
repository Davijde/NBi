using NBi.Extensibility;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Messaging
{
    public interface IRowCountFilteredPercentageMessenger
    {
        string WriteMessage(IResultSet actual, IResultSet filtered, ConstraintResult result);
    }
}
