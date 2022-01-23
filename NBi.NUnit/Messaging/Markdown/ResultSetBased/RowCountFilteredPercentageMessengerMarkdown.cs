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
    class RowCountFilteredPercentageMessengerMarkdown : RowCountFilteredMessengerMarkdown, IRowCountFilteredPercentageMessenger
    {
        public RowCountFilteredPercentageMessengerMarkdown(EngineStyle style, IDictionary<string, ISampler<IResultRow>> samplers)
            : base(style, samplers) { }
    }
}
