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
    abstract class ResultSetBasedMessengerMarkdown
    {
        protected readonly IDictionary<string, ISampler<IResultRow>> Samplers;
        protected readonly EngineStyle Style;

        public ResultSetBasedMessengerMarkdown(EngineStyle style, IDictionary<string, ISampler<IResultRow>> samplers)
            => (Style, Samplers) = (style, samplers);
        protected internal virtual void WriteActual(IResultSet expectedRs, StringBuilder writer)
            => BuildTable(expectedRs.Rows, Samplers["actual"], writer);

        //protected void BuildDuplication(IEnumerable<IResultRow> actualRows, ResultUniqueRows result)
        //{
        //    actual = new MarkdownContainer();
        //    var sb = new StringBuilder();
        //    var uniqueCount = actualRows.Count() - result.Rows?.Sum(x => Convert.ToInt32(x[0])) ?? 0;
        //    sb.Append($"The actual result-set has {result.RowCount} rows.");
        //    sb.Append($" {uniqueCount} row{(uniqueCount > 1 ? "s are" : " is")} effectively unique");
        //    sb.Append($" and {result.Values.Count()} distinct row{(result.Values.Count() > 1 ? "s are" : " is")} duplicated.");
        //    actual.Append(new Paragraph(sb.ToString()));
        //    actual.Append(BuildTable(style, actualRows, samplers["actual"]));
        //    analysis = new MarkdownContainer();
        //    analysis.Append(BuildNonEmptyTable(style, result.Rows, "Duplicated", samplers["analysis"]));
        //}

        //protected void BuildFilter(IEnumerable<IResultRow> actualRows, IEnumerable<IResultRow> filteredRows)
        //{
        //    actual = BuildTable(style, actualRows, samplers["actual"]);
        //    analysis = BuildTable(style, filteredRows, samplers["actual"]);
        //}
        //protected void BuildCount(IEnumerable<IResultRow> actualRows)
        //{
        //    actual = BuildTable(style, actualRows, samplers["actual"]);
        //}

        protected void BuildTable(IEnumerable<IResultRow> rows, ISampler<IResultRow> sampler, StringBuilder writer)
            => BuildTable(new TableHelperMarkdown(Style), rows, string.Empty, sampler, writer);

        protected void BuildTable(TableHelperMarkdown tableBuilder, IEnumerable<IResultRow> rows, string title, ISampler<IResultRow> sampler, StringBuilder writer)
        {
            rows = rows ?? new List<IResultRow>();

            sampler.Build(rows);
            var table = tableBuilder.Build(sampler.GetResult());

            var container = new MarkdownContainer();

            if (!string.IsNullOrEmpty(title))
            {
                var titleText = $"{title} rows:";
                container.Append(titleText.ToMarkdownSubHeader());
            }

            container.Append(BuildRowCount(rows.Count()));
            container.Append(table);

            if (sampler.GetIsSampled())
            {
                var rowsSkipped = $"{sampler.GetExcludedRowCount()} (of {rows.Count()}) rows have been skipped for display purpose.";
                container.Append(rowsSkipped.ToMarkdownParagraph());
            }

            writer.Append(container.ToMarkdown());
        }

        protected void BuildNonEmptyTable(IEnumerable<IResultRow> rows, string title, ISampler<IResultRow> sampler, StringBuilder writer)
        {
            var tableBuilder = new TableHelperMarkdown(Style);
            if (rows != null && rows.Count() > 0)
                BuildTable(tableBuilder, rows, title, sampler, writer);
            else
                writer.AppendLine(new MarkdownContainer().ToMarkdown());
        }


        //protected void BuildCompareTable(IEnumerable<IResultRow> rows, string title, ISampler<IResultRow> sampler, StringBuilder writer)
        //{
        //    var tableBuilder = new CompareTableHelperMarkdown(Style);
        //    if (rows.Count() > 0)
        //        BuildTable(tableBuilder, rows, title, sampler, writer);
        //    else
        //        writer.AppendLine(new MarkdownContainer().ToMarkdown());
        //}

        protected Paragraph BuildRowCount(int rowCount)
        {
            return ($"Result-set with {rowCount} row{(rowCount > 1 ? "s" : string.Empty)}".ToMarkdownParagraph());
        }
    }
}
