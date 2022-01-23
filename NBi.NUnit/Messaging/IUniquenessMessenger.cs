using NBi.Core.ResultSet.Discrimination;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Messaging
{
    public interface IUniquenessMessenger
    {
        string WriteMessage(IResultSet actual, ResultUniqueRows result);
    }
}
