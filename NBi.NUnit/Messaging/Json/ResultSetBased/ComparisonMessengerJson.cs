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
    class ComparisonMessengerJson : ResultSetBasedMessengerJson, IComparisonMessenger
    {
        public ComparisonMessengerJson(EngineStyle style, IDictionary<string, ISampler<IResultRow>> samplers)
            : base(style, samplers) { }

        public virtual string WriteMessage(IResultSet expectedRs, IResultSet actualRs, ResultResultSet result)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                writer.WriteStartObject();
                writer.WritePropertyName("timestamp");
                writer.WriteValue(DateTime.Now);
                WriteExpected(expectedRs, writer);
                WriteActual(actualRs, writer);
                WriteAnalysis(result, writer);
                writer.WriteEndObject();
                return sb.ToString();
            }
        }

        protected internal virtual void WriteExpected(IResultSet expectedRs, JsonWriter writer)
        {
            writer.WritePropertyName("expected");
            BuildTable(expectedRs.Rows, Samplers["expected"], writer);
        }
        
        protected internal virtual void WriteAnalysis(ResultResultSet result, JsonWriter writer)
        {
            writer.WritePropertyName("analysis");
            BuildMultipleTables(
                new[]
                {
                    new Tuple<string, IEnumerable<IResultRow>, TableHelperJson>("unexpected", result.Unexpected, new TableHelperJson()),
                    new Tuple<string, IEnumerable<IResultRow>, TableHelperJson>("missing", result.Missing, new TableHelperJson()),
                    new Tuple<string, IEnumerable<IResultRow>, TableHelperJson>("duplicated", result.Duplicated, new TableHelperJson()),
                    new Tuple<string, IEnumerable<IResultRow>, TableHelperJson>("non-matching", result.NonMatchingValue?.Rows ?? Enumerable.Empty<IResultRow>(), new CompareTableHelperJson()),
                }, Samplers["analysis"]
                , writer
             );
        }

        public void BuildFilter(IEnumerable<IResultRow> actualRows, IEnumerable<IResultRow> filteredRows)
        {
            //actual = BuildTable(actualRows, samplers["actual"]);
            //analysis = BuildMultipleTables(
            //    new[]
            //    {
            //        new Tuple<string, IEnumerable<IResultRow>, TableHelperJson>("filtered", filteredRows, new TableHelperJson())
            //    }, samplers["analysis"]);
        }
        public void BuildCount(IEnumerable<IResultRow> actualRows)
        {
            //actual = BuildTable(actualRows, samplers["actual"]);
        }
    }
}
