﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet
{
    class ColumnOrdinalIdentifier : IColumnIdentifier
    {
        public int Ordinal { get; private set; }

        public string Label => $"#{Ordinal.ToString()}";

        public ColumnOrdinalIdentifier(int position)
        {
            Ordinal = position;
        }

        public DataColumn GetColumn(DataTable dataTable) => dataTable.Columns[Ordinal];
        public object GetValue(DataRow dataRow) => dataRow[Ordinal];
    }
}
