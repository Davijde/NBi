using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NBi.Core.ResultSet;
using System.IO;
using static System.FormattableString;
using Newtonsoft.Json;
using NBi.Core.Sampling;
using NBi.Extensibility;
using NUnit.Framework.Constraints;
using NBi.NUnit.ResultSetBased.RowPredicate;

namespace NBi.NUnit.Messaging.Json.ResultSetBased
{
    class RowCountFilteredPercentageMessengerJson : RowCountFilteredMessengerJson, IRowCountFilteredPercentageMessenger
    {

        public RowCountFilteredPercentageMessengerJson(EngineStyle style, IDictionary<string, ISampler<IResultRow>> samplers)
            : base(style, samplers) { }

        protected override void WriteThreshold(ConstraintResult result, JsonWriter writer) 
        {
            (_, var percentage) = ((RowCountFilterPercentageConstraintResult)result).Constraint.Describe();

            writer.WritePropertyName("threshold");
            writer.WriteStartObject();
            writer.WritePropertyName("value");
            writer.WriteValue(percentage);
            writer.WritePropertyName("unit");
            writer.WriteValue("%");
            writer.WritePropertyName("display");
            writer.WriteValue(Invariant($"{percentage:F2}%"));
            writer.WriteEndObject();
        }
    }
}
