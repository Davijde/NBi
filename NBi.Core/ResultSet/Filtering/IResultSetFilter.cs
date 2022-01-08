using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.ResultSet.Alteration;

namespace NBi.Core.ResultSet.Filtering
{
    public interface IResultSetFilter : IAlteration
    {
        string Describe();
    }
}
