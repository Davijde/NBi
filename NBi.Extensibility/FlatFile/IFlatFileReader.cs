﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility.FlatFile
{
    public interface IFlatFileReader
    {
        event ProgressStatusHandler ProgressStatusChanged;
        DataTable ToDataTable(string filename);
        DataTable ToDataTable(Stream stream);
    }
}
