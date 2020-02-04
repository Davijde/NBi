﻿using NBi.Core.Calculation.Predication;
using NBi.Core.Variable;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Grouping.CaseBased
{
    class CaseGrouping : IGroupBy
    {
        protected IEnumerable<IPredication> Cases { get; }
        protected Context Context { get; }

        public CaseGrouping(IEnumerable<IPredication> cases, Context context)
            => (Cases, Context) = (cases, context);

        public IDictionary<ResultSet.KeyCollection, DataTable> Execute(ResultSet.ResultSet resultSet)
        {
            var stopWatch = new Stopwatch();
            var dico = new Dictionary<ResultSet.KeyCollection, DataTable>();
            stopWatch.Start();

            foreach (DataRow row in resultSet.Rows)
            {
                Context.Switch(row);
                var index = Cases.Select((p, i) => new { Predication = p, Index = i })
                                .FirstOrDefault(x => x.Predication.Execute(Context))
                                ?.Index ?? -1;
                var key = new ResultSet.KeyCollection(new object[] { index });
                if (!dico.ContainsKey(key))
                    dico.Add(key, row.Table.Clone());
                dico[key].ImportRow(row);
            }

            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Building rows' groups by cases: {dico.Count} [{stopWatch.Elapsed.ToString(@"d\d\.hh\h\:mm\m\:ss\s\ \+fff\m\s")}");
            return dico;
        }
    }
}
