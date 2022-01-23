using MarkdownLog;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Extensibility;
using NBi.NUnit.Messaging.Common;
using NBi.NUnit.Messaging.Common.Helper;
using NBi.NUnit.Messaging.Markdown.Helper;
using NBi.Core.Sampling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Messaging.Markdown
{
    class LookupReverseExistsViolationMessageMarkdown : LookupExistsViolationMessageMarkdown
    {

        public LookupReverseExistsViolationMessageMarkdown(IDictionary<string, ISampler<IResultRow>> samplers)
            : base(samplers) { }

    }
}
