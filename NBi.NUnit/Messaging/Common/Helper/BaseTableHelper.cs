using NBi.Core.ResultSet;
using NBi.Extensibility;
using NBi.Core.Sampling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.NUnit.Messaging.Common.Helper
{
    abstract class BaseTableHelper<T, U> : ITableHelper<U>
    {
        public IReadOnlyCollection<T> Rows { get; }
        public ISampler<T> Sampler { get; }
        public IEnumerable<ColumnMetadata> Metadatas { get; }

        public BaseTableHelper(IEnumerable<T> rows, IEnumerable<ColumnMetadata> metadata, ISampler<T> sampler)
            => (Rows, Metadatas, Sampler) = (new ReadOnlyCollection<T>(rows.ToList()), metadata, sampler);

        public abstract void Render(U writer);

        protected internal virtual IEnumerable<ExtendedMetadata> ExtendMetadata(IResultSet table, IEnumerable<ColumnMetadata> existingDefinitions)
        {
            var metadataDico = new Dictionary<IResultColumn, ColumnMetadata>();
            foreach (var definition in existingDefinitions)
                metadataDico.Add(definition.Identifier.GetColumn(table), definition);

            foreach (var dataColumn in table.Columns)
            {
                var metadata = metadataDico.ContainsKey(dataColumn) 
                    ? new ExtendedMetadata()
                    {
                        Ordinal = dataColumn.Ordinal,
                        Name = dataColumn.Name,
                        Role = metadataDico[dataColumn].Role,
                        Type = metadataDico[dataColumn].Type
                    }
                    : new ExtendedMetadata()
                    {
                        Ordinal = dataColumn.Ordinal,
                        Name = dataColumn.Name,
                        Role = ColumnRole.Ignore,
                        Type = ColumnType.Text
                    };
                yield return metadata;
            }
        }

        protected internal class ExtendedMetadata : ColumnMetadata
        {
            public string Name { get; set; }
            public int Ordinal { get; set; }
        }
    }
}
