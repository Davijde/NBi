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
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;

namespace NBi.NUnit.Messaging.Json.ResultSetBased
{
    class RowCountFilteredMessengerJson : RowCountMessengerJson, IRowCountFilteredMessenger
    {

        public RowCountFilteredMessengerJson(EngineStyle style, IDictionary<string, ISampler<IResultRow>> samplers)
            : base(style, samplers) { }

        public virtual string WriteMessage(IResultSet actual, IResultSet filtered, ConstraintResult result)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();
                writer.WritePropertyName("timestamp");
                writer.WriteValue(DateTime.Now);
                WriteActual(actual, writer);
                WriteAnalysis(result, filtered, writer);
                writer.WriteEndObject();
                return sb.ToString();
            }
        }

        protected internal void WriteAnalysis(ConstraintResult result, IResultSet filtered, JsonWriter writer)
        {
            writer.WritePropertyName("analysis");
            writer.WriteStartObject();
            WriteFilter(filtered, writer);
            WriteConstraint(result, writer);
            WriteResult(result, writer);
            writer.WriteEndObject();
        }

        protected void WriteFilter(IResultSet filtered, JsonWriter writer)
        {
            writer.WritePropertyName("predication-filter");
            BuildTable(filtered.Rows, Samplers["analysis"], writer);
        }
    }
}
