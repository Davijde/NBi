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
using NUnit.Framework.Constraints;

namespace NBi.NUnit.Messaging.Markdown.ResultSetBased
{
    class RowCountFilteredMessengerMarkdown : ResultSetBasedMessengerMarkdown, IRowCountFilteredMessenger
    {
        public RowCountFilteredMessengerMarkdown(EngineStyle style, IDictionary<string, ISampler<IResultRow>> samplers)
            : base(style, samplers) { }

        public virtual string WriteMessage(IResultSet actual, IResultSet filtered, ConstraintResult result)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Result-set has {result.Description}.");
            sb.AppendLine();
            WriteActual(actual, sb);
            sb.AppendLine();
            WriteAnalysis(filtered, sb);
            return sb.ToString();
        }

        protected internal virtual void WriteAnalysis(IResultSet filtered, StringBuilder writer)
        {
            BuildTable(filtered.Rows, Samplers["analysis"], writer);
        }
    }
}
