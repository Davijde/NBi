using NBi.NUnit.Messaging.Markdown;
using NBi.Core.Sampling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Messaging.Json
{
    class ItemsMessageJson : ItemsMessageMarkdown
    {
        public ItemsMessageJson(IDictionary<string, ISampler<string>> samplers)
            : base(samplers)
        {
        }
    }
}
