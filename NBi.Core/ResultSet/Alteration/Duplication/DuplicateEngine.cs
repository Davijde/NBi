﻿using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Predication;
using NBi.Core.Injection;
using NBi.Core.Variable;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Duplication
{
    class DuplicateEngine : IDuplicationEngine
    {
        protected ServiceLocator ServiceLocator { get; }
        protected Context Context { get; }
        protected IPredication Predication { get; }
        protected IScalarResolver<int> Times { get; }
        protected IList<OutputArgs> Outputs { get; }

        public DuplicateEngine(ServiceLocator serviceLocator, Context context, IPredication predication, IScalarResolver<int> times, IList<OutputArgs> outputs)
            => (ServiceLocator, Context, Predication, Times, Outputs) = (serviceLocator, context, predication, times, outputs);

        public IResultSet Execute(IResultSet rs)
        {
            var result = rs.Clone();
            result.Clear();

            //Add the new columns
            foreach (var output in Outputs)
            {
                if (result.GetColumn(output.Identifier) == null)
                {
                    switch (output.Identifier)
                    {
                        case ColumnNameIdentifier identifier:
                            result.Columns.Add(new DataColumn(identifier.Name, typeof(object)) { DefaultValue = DBNull.Value });
                            break;
                        case ColumnOrdinalIdentifier identifier:
                            result.Columns.Add(new DataColumn($"Column_{identifier.Ordinal}", typeof(object)) { DefaultValue = DBNull.Value });
                            break;
                        default:
                            break;
                    }
                }
            }

            foreach (DataRow row in rs.Rows)
            {
                Context.Switch(row);
                var isDuplicated = Predication.Execute(Context);
                var times = Times.Execute();

                result.ImportRow(row);
                foreach (var output in Outputs)
                {
                    if (output.Strategy.IsApplicable(true))
                    {
                        var columnName = result.GetColumn(output.Identifier).ColumnName;
                        result.Rows[result.Rows.Count - 1][columnName] = output.Strategy.Execute(true, isDuplicated, times, 0);
                    }
                }

                if (isDuplicated)
                {
                    for (int i = 0; i < times; i++)
                    {
                        result.ImportRow(row);
                        Context.Switch(row);
                        foreach (var output in Outputs)
                        {
                            if (output.Strategy.IsApplicable(false))
                            {
                                var columnName = result.GetColumn(output.Identifier).ColumnName;
                                result.Rows[result.Rows.Count - 1][columnName] = output.Strategy.Execute(false, true, times, i);
                                Context.Switch(result.Rows[result.Rows.Count - 1]);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
