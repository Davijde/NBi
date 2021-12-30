using NBi.Core.ResultSet.Analyzer;
using NBi.Extensibility;
using System.Collections.Generic;

namespace NBi.Core.ResultSet.Equivalence
{
    public interface IEquivaler
    {
        IEquivaler Using(IEnumerable<IRowsAnalyzer> analyzers);

        ResultResultSet Compare(IResultSet x, IResultSet y);
        ISettingsResultSet Settings { get; set; }
        EngineStyle Style { get; }
    }
}
