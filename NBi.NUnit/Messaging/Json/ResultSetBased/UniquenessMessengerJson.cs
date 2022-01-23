using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NBi.Core.ResultSet;
using System.IO;
using Newtonsoft.Json;
using NBi.Core.Sampling;
using NBi.Core.ResultSet.Uniqueness;
using NBi.Extensibility;
using NBi.Core.ResultSet.Discrimination;

namespace NBi.NUnit.Messaging.Json.ResultSetBased
{
    class UniquenessMessengerJson : ResultSetBasedMessengerJson, IUniquenessMessenger
    {

        public UniquenessMessengerJson(EngineStyle style, IDictionary<string, ISampler<IResultRow>> samplers)
            : base(style, samplers) { }

        public virtual string WriteMessage(IResultSet actualRs, ResultUniqueRows result)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();
                writer.WritePropertyName("timestamp");
                writer.WriteValue(DateTime.Now);
                WriteActual(actualRs, writer);
                WriteAnalysis(result, writer);
                writer.WriteEndObject();
                return sb.ToString();
            }
        }

        protected internal void WriteAnalysis(ResultUniqueRows result, JsonWriter writer)
        {
            writer.WritePropertyName("analysis");
            BuildMultipleTables(
                new[]
                {
                    new Tuple<string, IEnumerable<IResultRow>, TableHelperJson>("duplicated", result.Rows, new TableHelperJson()),
                }, Samplers["analysis"]
                , writer
             );
        }
    }
}
