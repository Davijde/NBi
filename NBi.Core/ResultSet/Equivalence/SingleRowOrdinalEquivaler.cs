﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NBi.Core.ResultSet.Analyzer;
using NBi.Extensibility;

namespace NBi.Core.ResultSet.Equivalence
{
    public class SingleRowOrdinalEquivaler : OrdinalEquivaler
    {
        private new SettingsSingleRowOrdinalResultSet  Settings
        {
            get { return base.Settings as SettingsSingleRowOrdinalResultSet ; }
        }
        
        public SingleRowOrdinalEquivaler(SettingsSingleRowOrdinalResultSet  settings)
            : base(AnalyzersFactory.EqualTo(), settings)
        {}

        protected override ResultResultSet doCompare(IResultSet x, IResultSet y)
        {
            if (x.Rows.Count > 1)
                throw new ArgumentException(string.Format("The query in the assertion returns {0} rows. It was expected to return zero or one row.", x.Rows.Count));

            if (y.Rows.Count > 1)
                throw new ArgumentException(string.Format("The query in the system-under-test returns {0} rows. It was expected to return zero or one row.", y.Rows.Count));
            
            if (x.Rows.Count == 1 && y.Rows.Count == 1)
                PreliminaryChecks(x, y);
            
            return doCompare(x.Rows.Count == 1 ? x.Rows[0] : null, y.Rows.Count == 1 ? y.Rows[0] : null);
        }

        protected ResultResultSet doCompare(DataRow x, DataRow y)
        {
            var chrono = DateTime.Now;

            var missingRows = new List<DataRow>();
            var unexpectedRows = new List<DataRow>();

            if (x == null && y != null)
                unexpectedRows.Add(y);

            if (x != null && y == null)
                missingRows.Add(x);
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Analyzing length of result-sets: [{DateTime.Now.Subtract(chrono):d\\d\\.hh\\h\\:mm\\m\\:ss\\s\\ \\+fff\\m\\s}]");

            IList<DataRow> nonMatchingValueRows = new List<DataRow>();
            if (missingRows.Count == 0 && unexpectedRows.Count == 0)
            {
                chrono = DateTime.Now;
                var columnsCount = Math.Max(y.Table.Columns.Count, x.Table.Columns.Count);
                if (Settings == null)
                    BuildDefaultSettings(columnsCount);
                else
                    Settings.ApplyTo(columnsCount);

                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Analyzing length and format of result-sets: [{DateTime.Now.Subtract(chrono):d\\d\\.hh\\h\\:mm\\m\\:ss\\s\\ \\+fff\\m\\s}]");

                // If all of the columns make up the key, then we already know which rows match and which don't.
                //  So there is no need to continue testing
                chrono = DateTime.Now;
                var nonMatchingValueRow = CompareRows(x, y);
                if (nonMatchingValueRow!=null)
                    nonMatchingValueRows.Add(nonMatchingValueRow);
                Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"Rows with a matching key but without matching value: {nonMatchingValueRows.Count()} [{DateTime.Now.Subtract(chrono):d\\d\\.hh\\h\\:mm\\m\\:ss\\s\\ \\+fff\\m\\s}]");
            }

            return ResultResultSet.Build(
                missingRows,
                unexpectedRows,
                new List<DataRow>(),
                new List<DataRow>(),
                nonMatchingValueRows
                );
        }
        
    }
}
