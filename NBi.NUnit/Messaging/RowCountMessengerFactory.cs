using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Extensibility;
using NBi.NUnit.Messaging.Json.ResultSetBased;
using NBi.NUnit.Messaging.Markdown.ResultSetBased;
using NBi.Core.Sampling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Messaging
{
    public class RowCountMessengerFactory
    {
        public IRowCountMessenger Instantiate(IFailureReportProfile profile, Core.ResultSet.EngineStyle style)
        {
            var factory = new SamplersFactory<IResultRow>();
            var samplers = factory.Instantiate(profile);

            switch (profile.Format)
            {
                case FailureReportFormat.Markdown:
                    return new RowCountMessengerMarkdown(style, samplers);
                case FailureReportFormat.Json:
                    return new RowCountMessengerJson(style, samplers);
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }
}
