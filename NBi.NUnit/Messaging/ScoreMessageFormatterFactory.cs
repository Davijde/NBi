using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.NUnit.Messaging.Json;
using NBi.NUnit.Messaging.Markdown;
using NBi.Core.Sampling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Messaging
{
    public class ScoreMessageFormatterFactory
    {
        public IScoreMessageFormatter Instantiate(IFailureReportProfile profile)
        {
            switch (profile.Format)
            {
                case FailureReportFormat.Markdown:
                    return new ScoreMessageMarkdown();
                case FailureReportFormat.Json:
                    return new ScoreMessageJson();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }
}
