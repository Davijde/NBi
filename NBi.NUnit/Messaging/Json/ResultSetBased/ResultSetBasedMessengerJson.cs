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
    abstract class ResultSetBasedMessengerJson
    {
        protected IDictionary<string, ISampler<IResultRow>> Samplers { get; }
        protected EngineStyle Style { get; }

        public ResultSetBasedMessengerJson(EngineStyle style, IDictionary<string, ISampler<IResultRow>> samplers)
            => (Style, Samplers) = (style, samplers);

        protected internal virtual void WriteActual(IResultSet expectedRs, JsonWriter writer)
        {
            writer.WritePropertyName("actual");
            BuildTable(expectedRs.Rows, Samplers["actual"], writer);
        }

        protected virtual void BuildTable(IEnumerable<IResultRow> rows, ISampler<IResultRow> sampler, JsonWriter writer)
            => BuildTable(rows, sampler, new TableHelperJson(), writer);

        private void BuildTable(IEnumerable<IResultRow> rows, ISampler<IResultRow> sampler, TableHelperJson tableHelper, JsonWriter writer)
            => tableHelper.Execute(rows, sampler, writer);

        protected internal virtual void BuildMultipleTables(IEnumerable<Tuple<string, IEnumerable<IResultRow>, TableHelperJson>> tableInfos, ISampler<IResultRow> sampler, JsonWriter writer)
        {
            writer.WriteStartObject();
            foreach (var item in tableInfos)
            {
                writer.WritePropertyName(item.Item1);
                BuildTable(item.Item2, sampler, item.Item3, writer);
            }
            writer.WriteEndObject();
        }

    }
}
