using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.NUnit.Messaging.Json;
using NBi.NUnit.Messaging.Markdown;
using NBi.Core.Sampling;
using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using NBi.Extensibility;

namespace NBi.NUnit.Messaging
{
    public class LookupExistsViolationsMessageFormatterFactory
    {
        public ILookupViolationMessageFormatter Instantiate(IFailureReportProfile profile)
        {
            var dataRowsFactory = new SamplersFactory<IResultRow>();
            var dataRowsSamplers = dataRowsFactory.InstantiateLookup(profile);

            var keysCollectionFactory = new SamplersFactory<KeyCollection>();
            var keysCollectionSamplers = keysCollectionFactory.Instantiate(profile);

            switch (profile.Format)
            {
                case FailureReportFormat.Markdown:
                    return new LookupExistsViolationMessageMarkdown(dataRowsSamplers);
                case FailureReportFormat.Json:
                    return new LookupExistsViolationMessageJson(dataRowsSamplers);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
