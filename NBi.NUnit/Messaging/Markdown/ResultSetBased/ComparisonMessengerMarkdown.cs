using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.NUnit.Messaging.Markdown.Helper;
using NBi.Core.Sampling;
using NBi.Core.ResultSet.Uniqueness;
using NBi.Extensibility;
using NBi.Core.ResultSet.Discrimination;

namespace NBi.NUnit.Messaging.Markdown.ResultSetBased
{
    class ComparisonMessengerMarkdown : ResultSetBasedMessengerMarkdown, IComparisonMessenger
    {
        public ComparisonMessengerMarkdown(EngineStyle style, IDictionary<string, ISampler<IResultRow>> samplers)
            : base(style, samplers) { }

        public string WriteMessage(IResultSet expected, IResultSet actual, ResultResultSet result)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Execution of the query doesn't match the expected result");
            sb.AppendLine();
            WriteExpected(expected, sb);
            sb.AppendLine();
            WriteActual(actual, sb);
            sb.AppendLine();
            WriteAnalysis(result, sb);
            return sb.ToString();
        }

        protected internal virtual void WriteExpected(IResultSet expectedRs, StringBuilder writer)
            => BuildTable(expectedRs.Rows, Samplers["expected"], writer);
        
        protected internal virtual void WriteAnalysis(ResultResultSet result, StringBuilder writer)
        {
            BuildNonEmptyTable(result.Unexpected ?? new List<IResultRow>(), "Unexpected", Samplers["analysis"], writer);
            BuildNonEmptyTable(result.Missing ?? new List<IResultRow>(), "Missing", Samplers["analysis"], writer);
            BuildNonEmptyTable(result.Duplicated ?? new List<IResultRow>(), "Duplicated", Samplers["analysis"], writer);
            BuildCompareTable(result?.NonMatchingValue?.Rows ?? new List<IResultRow>(), "Non matching value", Samplers["analysis"], writer);
        }

        protected void BuildCompareTable(IEnumerable<IResultRow> rows, string title, ISampler<IResultRow> sampler, StringBuilder writer)
        {
            var tableBuilder = new CompareTableHelperMarkdown(Style);
            if (rows.Count() > 0)
                BuildTable(tableBuilder, rows, title, sampler, writer);
            else
                writer.AppendLine(new MarkdownContainer().ToMarkdown());
        }
    }
}
