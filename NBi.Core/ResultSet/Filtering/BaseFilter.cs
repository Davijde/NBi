using NBi.Extensibility;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering
{
    public abstract class BaseFilter : IPredicateFilter
    {
        protected Context Context { get; }
        protected Func<IResultSet, IResultSet> Execution { get; private set; }

        protected BaseFilter(Context context)
        => (Context, Execution) = (context, Keep);

        public IResultSet Execute(IResultSet rs)
            => Execution.Invoke(rs);

        protected IResultSet Discard(IResultSet rs) 
            => Execute(rs, (x => !x));

        protected IResultSet Keep(IResultSet rs) 
            => Execute(rs, (x => x));

        public IResultSetFilter Revert()
        {
            Execution = Execution == Keep ? (Func<IResultSet, IResultSet>)Discard : Keep;
            return this;
        }


                protected IResultSet Execute(IResultSet rs, Func<bool, bool> onApply)
                {
                    var table = rs.Clone();
                    table.Clear();

                    foreach (var row in rs.Rows)
                    {
                        Context.Switch(row);
                        if (onApply(RowApply(Context)))
                        {
                            if (table.RowCount == 0 && table.ColumnCount != rs.ColumnCount)
                            {
                                foreach (var column in rs.Columns)
                                {
                                    if (!table.ContainsColumn(column.Name))
                                        table.AddColumn(column.Name);
                                }
                            }
                            table.AddRow(row);
                        }
                    }

                    table.AcceptChanges();
                    return table;
                }

        protected abstract bool RowApply(Context context);

        public abstract string Describe();
    }
}