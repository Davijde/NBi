using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Discrimination
{
    public class UniquenessFactory
    {
        public UniquenessEngine Instantiate(ISettingsResultSet settings)
        {
            if (settings is SettingsOrdinalResultSet)
                return new OrdinalUniquenessEngine(settings as SettingsOrdinalResultSet);
            else if (settings is SettingsNameResultSet)
                return new NameUniquenessEngine(settings as SettingsNameResultSet);
            throw new ArgumentOutOfRangeException();
        }
    }
}
