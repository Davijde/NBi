﻿using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Transformation;
using NBi.Core.Transformation.Transformer;
using NBi.Core.Variable;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    public class RowValueExtractor
    {
        private ServiceLocator ServiceLocator { get; }

        public RowValueExtractor(ServiceLocator serviceLocator)
            => (ServiceLocator) = (serviceLocator);

        public object Execute(Context context, IColumnIdentifier identifier)
        {
            if (identifier is ColumnOrdinalIdentifier)
            {
                var ordinal = (identifier as ColumnOrdinalIdentifier).Ordinal;
                if (ordinal <= context.CurrentRow.Table.Columns.Count)
                    return context.CurrentRow.ItemArray[ordinal];
                else
                    throw new ArgumentException($"The variable of the predicate is identified as '{identifier.Label}' but the column in position '{ordinal}' doesn't exist. The dataset only contains {context.CurrentRow.Table.Columns.Count} columns.");
            }

            var name = (identifier as ColumnNameIdentifier).Name;
            var alias = context.Aliases?.SingleOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
            if (alias != null)
                return context.CurrentRow.ItemArray[alias.Column];

            var expression = context.Expressions?.SingleOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
            if (expression != null)
            {
                var result = EvaluateExpression(expression, context);
                var expColumnName = $"exp::{name}";
                if (!context.CurrentRow.Table.Columns.Contains(expColumnName))
                {
                    var newColumn = new DataColumn(expColumnName, typeof(object));
                    context.CurrentRow.Table.Columns.Add(newColumn);
                }

                context.CurrentRow[expColumnName] = result;
                return result;
            }

            var column = context.CurrentRow.Table.Columns.Cast<DataColumn>().SingleOrDefault(x => string.Equals(x.ColumnName, name, StringComparison.OrdinalIgnoreCase));
            if (column != null)
                return context.CurrentRow[column.ColumnName];

            var existingNames = context.CurrentRow.Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName)
                .Union(context.Aliases.Select(x => x.Name)
                .Union(context.Expressions.Select(x => x.Name)));

            throw new ArgumentException($"The value '{name}' is not recognized as a column position, a column name, a column alias or an expression. Possible arguments are: '{string.Join("', '", existingNames.ToArray())}'");
        }

        protected object EvaluateExpression(IColumnExpression expression, Context context)
        {
            if (expression.Language == LanguageType.NCalc)
            {
                var exp = new NCalc.Expression(expression.Value);
                var factory = new ColumnIdentifierFactory();

                exp.EvaluateParameter += delegate (string name, NCalc.ParameterArgs args)
                {
                    args.Result = name.StartsWith("@")
                        ? context.Variables[name.Substring(1, name.Length-1)].GetValue()
                        : Execute(context, factory.Instantiate(name));
                };

                return exp.Evaluate();
            }
            else if (expression.Language == LanguageType.Native)
            {
                var parse = expression.Value.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                var variable = new ColumnIdentifierFactory().Instantiate(parse.ElementAt(0));
                var value = Execute(context, variable);

                foreach (var nativeFunction in parse.Skip(1))
                {
                    var factory = new NativeTransformationFactory(ServiceLocator, context);
                    var transformation = factory.Instantiate(nativeFunction);
                    value = transformation.Evaluate(value);
                }
                
                return value;
            }
            else
                throw new ArgumentOutOfRangeException($"The language {expression.Language} is not supported during the evaluation of an expression.");
        }

        private class TransformationInfo : ITransformationInfo
        {
            public ColumnType OriginalType { get; set; }
            public LanguageType Language { get; set; }
            public string Code { get; set; }
        }
    }
}
