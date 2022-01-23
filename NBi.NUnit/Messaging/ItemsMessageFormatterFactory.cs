using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.NUnit.Messaging.Json;
using NBi.NUnit.Messaging.Markdown;
using NBi.Core.Sampling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Messaging
{
    public class ItemsMessageFormatterFactory
    {
        public IItemsMessageFormatter Instantiate(IFailureReportProfile profile)
        {
            var factory = new SamplersFactory<string>();
            var samplers = factory.Instantiate(profile);

            switch (profile.Format)
            {
                case FailureReportFormat.Markdown:
                    return new ItemsMessageMarkdown(samplers);
                case FailureReportFormat.Json:
                    return new ItemsMessageJson(samplers);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
