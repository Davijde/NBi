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
    class UniquenessMessengerMarkdown : ResultSetBasedMessengerMarkdown, IUniquenessMessenger
    {
        public UniquenessMessengerMarkdown(EngineStyle style, IDictionary<string, ISampler<IResultRow>> samplers)
            : base(style, samplers) { }

        public virtual string WriteMessage(IResultSet actual, ResultUniqueRows result)
        {
            var sb = new StringBuilder();
            sb.AppendLine("All rows are unique");
            sb.AppendLine();
            WriteActual(actual, sb);
            sb.AppendLine();
            WriteAnalysis(result, sb);
            return sb.ToString();
        }

        protected internal virtual void WriteAnalysis(ResultUniqueRows result, StringBuilder writer)
        {
            var text = result.Rows.Count()==1 ? "set of rows is" : "sets of rows are";
            writer.Append($"{result.Rows.Count()} {text} not unique.".ToMarkdownParagraph().ToString());
            BuildNonEmptyTable(result.Rows ?? new List<IResultRow>(), "Duplicated", Samplers["analysis"], writer);
        }
    }
}
