﻿using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Ranking.Scoring
{
    class DataRowScorer : IScorer<IResultRow>
    {
        protected readonly IColumnIdentifier operand;
        protected readonly IEnumerable<IColumnExpression> expressions;
        protected readonly IEnumerable<IColumnAlias> aliases;

        public DataRowScorer(IColumnIdentifier operand, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions)
        {
            this.operand = operand;
            this.aliases = aliases;
            this.expressions = expressions;
        }

        public ScoredObject Execute(IResultRow row)
            => new ScoredObject(GetValueFromRow(row, operand), row);

        protected object GetValueFromRow(IResultRow row, IColumnIdentifier identifier)
        {
            if (identifier is ColumnOrdinalIdentifier)
            {
                var ordinal = (identifier as ColumnOrdinalIdentifier).Ordinal;
                if (ordinal <= row.Parent.ColumnCount)
                    return row.ItemArray[ordinal];
                else
                    throw new ArgumentException($"The variable of the predicate is identified as '{identifier.Label}' but the column in position '{ordinal}' doesn't exist. The dataset only contains {row.Parent.ColumnCount} columns.");
            }

            var name = (identifier as ColumnNameIdentifier).Name;
            var alias = aliases?.SingleOrDefault(x => x.Name == name);
            if (alias != null)
                return row.ItemArray[alias.Column];

            var expression = expressions?.SingleOrDefault(x => x.Name == name);
            if (expression != null)
                return EvaluateExpression(expression, row);

            var column = row.Parent.GetColumn(name);
            if (column != null)
                return row[column.Name];

            throw new ArgumentException($"The value '{name}' is not recognized as a column name or a column position or a column alias or an expression.");
        }

        protected object EvaluateExpression(IColumnExpression expression, IResultRow row)
        {
            var exp = new NCalc.Expression(expression.Value);
            var factory = new ColumnIdentifierFactory(); 

            exp.EvaluateParameter += delegate (string name, NCalc.ParameterArgs args)
            {
                args.Result = GetValueFromRow(row, factory.Instantiate(name));
            };

            return exp.Evaluate();
        }
    }
}
