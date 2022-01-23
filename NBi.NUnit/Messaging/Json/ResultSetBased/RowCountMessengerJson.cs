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
    class RowCountMessengerJson : ResultSetBasedMessengerJson, IRowCountMessenger
    {

        public RowCountMessengerJson(EngineStyle style, IDictionary<string, ISampler<IResultRow>> samplers)
            : base(style, samplers) { }

        public virtual string WriteMessage(IResultSet actual, ConstraintResult result)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();
                writer.WritePropertyName("timestamp");
                writer.WriteValue(DateTime.Now);
                WriteActual(actual, writer);
                WriteAnalysis(result, writer);
                writer.WriteEndObject();
                return sb.ToString();
            }
        }

        protected internal void WriteAnalysis(ConstraintResult result, JsonWriter writer)
        {
            writer.WritePropertyName("analysis");
            writer.WriteStartObject();
            WriteConstraint(result, writer);
            WriteResult(result, writer);
            writer.WriteEndObject();
        }

        protected void WriteConstraint(ConstraintResult result, JsonWriter writer)
        {
            writer.WritePropertyName("constraint");
            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteValue(result.Name);
            writer.WritePropertyName("description");
            writer.WriteValue(result.Description);
            writer.WriteEndObject();
        }

        protected void WriteResult(ConstraintResult result, JsonWriter writer)
        {
            writer.WritePropertyName("result");
            writer.WriteStartObject();
            writer.WritePropertyName("row-count");
            writer.WriteValue(result.ActualValue);
            writer.WritePropertyName("message");
            writer.WriteValue(GetUnderlyingMessage(result.WriteMessageTo));
            writer.WriteEndObject();
        }

        protected string GetUnderlyingMessage(Action<MessageWriter> write)
        {
            using (var writer = new TextMessageWriter())
            {
                write(writer);
                writer.Flush();
                return writer.ToString().TrimStart();   
            }
        }
    }
}
