using Moq;
using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Extensibility;
using NBi.NUnit.Messaging.Markdown.ResultSetBased;
using NBi.Core.Sampling;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.NUnit.Messaging.Markdown.ResultSetBased
{
    public class ComparisonMessengerMarkdownTest
    {
        #region Helpers
        private IEnumerable<IResultRow> GetDataRows(int count)
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < count; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);
            var rs = new DataTableResultSet(dataTable);

            return rs.Rows;
        }
        #endregion

        [Test]
        public void WriteExpected_MoreThanMaxRowsCount_ReturnCorrectNumberOfRowsOnTop()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 20; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);
            var rs = new DataTableResultSet(dataTable);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new ComparisonMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var writer = new StringBuilder();
            msg.WriteExpected(rs, writer);
            var lines = writer.Replace("\n", string.Empty).ToString().Split('\r');

            Assert.That(lines[0], Is.EqualTo("Result-set with 20 rows"));
        }

        [Test]
        public void WriteExpected_OneRow_ReturnCorrectNumberOfRowsOnTopWithoutPlurial()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 1; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);
            var rs = new DataTableResultSet(dataTable);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new ComparisonMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var writer = new StringBuilder();
            msg.WriteExpected(rs, writer);
            var lines = writer.Replace("\n", string.Empty).ToString().Split('\r');

            Assert.That(lines[0], Is.EqualTo("Result-set with 1 row"));
        }

        [Test]
        public void WriteExpected_MoreThanMaxRowsCount_ReturnSampleRowsCountAndHeadersAndSeparation()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.Columns["Id"].ExtendedProperties.Add("NBi::Role", ColumnRole.Key);
            for (int i = 0; i < 20; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);
            var rs = new DataTableResultSet(dataTable);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new ComparisonMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var writer = new StringBuilder();
            msg.WriteExpected(rs, writer);
            var lines = writer.Replace("\n", string.Empty).ToString().Split('\r');

            Assert.That(lines.Count(l => l.Contains("|")), Is.EqualTo(10 + 3));
        }

        [Test]
        public void WriteExpected_MoreThanMaxRowsCount_ReturnSampleRowsCountAndHeaderAndSeparation()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 20; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);
            var rs = new DataTableResultSet(dataTable);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new ComparisonMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var writer = new StringBuilder();
            msg.WriteExpected(rs, writer);
            var lines = writer.Replace("\n", string.Empty).ToString().Split('\r');

            Assert.That(lines.Count(l => l.Contains("|")), Is.EqualTo(10 + 3 -1)); //-1 because we've no ExtendedProperties
        }

        [Test]
        public void WriteExpected_MoreThanSampleRowsCountButLessThanMaxRowsCount_ReturnEachRowAndHeaderAndSeparation()
        {
            var rowCount = 12;

            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.Columns["Id"].ExtendedProperties.Add("NBi::Role", ColumnRole.Key);
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);
            var rs = new DataTableResultSet(dataTable);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new ComparisonMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var writer = new StringBuilder();
            msg.WriteExpected(rs, writer);
            var lines = writer.Replace("\n", string.Empty).ToString().Split('\r');

            Assert.That(lines.Count(l => l.Contains("|")), Is.EqualTo(rowCount + 3));
        }

        [Test]
        public void WriteExpected_MoreThanSampleRowsCountButLessThanMaxRowsCountWithSpecificProfile_ReturnEachRowAndHeaderAndSeparation()
        {
            var rowCount = 120;
            var threshold = rowCount - 20;
            var max = threshold / 2;

            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.Columns["Id"].ExtendedProperties.Add("NBi::Role", ColumnRole.Key);
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);
            var rs = new DataTableResultSet(dataTable);

            var profile = Mock.Of<IFailureReportProfile>(p =>
                p.MaxSampleItem == max
                && p.ThresholdSampleItem == threshold
                && p.ExpectedSet == FailureReportSetType.Sample
            );

            var samplers = new SamplersFactory<IResultRow>().Instantiate(profile);
            var msg = new ComparisonMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var writer = new StringBuilder();
            msg.WriteExpected(rs, writer);
            var lines = writer.Replace("\n", string.Empty).ToString().Split('\r');

            Assert.That(lines.Count(l => l.Contains("|")), Is.EqualTo(max + 3));
        }

        [Test]
        public void WriteExpected_MoreThanSampleRowsCountButLessThanMaxRowsCountWithSpecificProfileFull_ReturnEachRowAndHeaderAndSeparation()
        {
            var rowCount = 120;
            var threshold = rowCount - 20;
            var max = threshold / 2;

            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            dataTable.Columns["Id"].ExtendedProperties.Add("NBi::Role", ColumnRole.Key);
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);
            var rs = new DataTableResultSet(dataTable);

            var profile = Mock.Of<IFailureReportProfile>(p =>
                p.MaxSampleItem == max
                && p.ThresholdSampleItem == threshold
                && p.ExpectedSet == FailureReportSetType.Full
            );

            var samplers = new SamplersFactory<IResultRow>().Instantiate(profile);
            var msg = new ComparisonMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var writer = new StringBuilder();
            msg.WriteExpected(rs, writer);
            var lines = writer.Replace("\n", string.Empty).ToString().Split('\r');

            Assert.That(lines.Count(l => l.Contains("|")), Is.EqualTo(rowCount + 3));
        }

        [Test]
        public void WriteExpected_MoreThanSampleRowsCountButLessThanMaxRowsCountWithSpecificProfileNone_ReturnEachRowAndHeaderAndSeparation()
        {
            var rowCount = 120;
            var threshold = rowCount - 20;
            var max = threshold / 2;

            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);
            var rs = new DataTableResultSet(dataTable);

            var profile = Mock.Of<IFailureReportProfile>(p =>
                p.MaxSampleItem == max
                && p.ThresholdSampleItem == threshold
                && p.ExpectedSet == FailureReportSetType.None
            );

            var samplers = new SamplersFactory<IResultRow>().Instantiate(profile);
            var msg = new ComparisonMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var writer = new StringBuilder();
            msg.WriteExpected(rs, writer);
            var lines = writer.Replace("\n", string.Empty).ToString().Split('\r');

            Assert.That(lines.Count(l => l.Contains("|")), Is.EqualTo(0));
            //Assert.That(lines, Has.All.EqualTo("Display skipped."));
        }

        [Test]
        public void WriteExpected_MoreThanMaxRowsCount_ReturnCorrectCountOfSkippedRow()
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 22; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);
            var rs = new DataTableResultSet(dataTable);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new ComparisonMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var writer = new StringBuilder();
            msg.WriteExpected(rs, writer);
            var lines = writer.Replace("\n", string.Empty).ToString().Split('\r');
            //Not exactly the last line but the previous due to paragraph creating additional line.
            var lastLine = lines.Reverse().ElementAt(1);

            Assert.That(lastLine, Is.EqualTo("12 (of 22) rows have been skipped for display purpose."));
        }

        [Test]
        [TestCase(5)]
        [TestCase(12)]
        public void WriteExpected_LessThanMaxRowsCount_DoesntDisplaySkippedRow(int rowCount)
        {
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);
            var rs = new DataTableResultSet(dataTable);

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new ComparisonMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var writer = new StringBuilder();
            msg.WriteExpected(rs, writer);

            Assert.That(writer.ToString(), Does.Not.Contain("rows have been skipped for display purpose."));
        }

        [Test]
        [TestCase(0, 5, 5, 5, 5, "Missing rows:")]
        [TestCase(5, 0, 5, 5, 5, "Unexpected rows:")]
        [TestCase(5, 5, 0, 5, 5, "Duplicated rows:")]
        [TestCase(5, 5, 5, 5, 0, "Non matching value rows:")]
        public void WriteCompared_NoSpecialRows_DoesntDisplayTextForThisKindOfRows(
            int missingRowCount
            , int unexpectedRowCount
            , int duplicatedRowCount
            , int keyMatchingRowCount
            , int nonMatchingValueRowCount
            , string unexpectedText)
        {
            var compared = ResultResultSet.Build(
                    GetDataRows(missingRowCount)
                    , GetDataRows(unexpectedRowCount)
                    , GetDataRows(duplicatedRowCount)
                    , GetDataRows(keyMatchingRowCount)
                    , GetDataRows(nonMatchingValueRowCount)
                );

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new ComparisonMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var writer = new StringBuilder();
            msg.WriteAnalysis(compared, writer);

            Assert.That(writer.ToString(), Does.Not.Contain(unexpectedText));
        }

        [Test]
        [TestCase(3, 0, 0, 0, 0, "Missing rows:")]
        [TestCase(0, 3, 0, 0, 0, "Unexpected rows:")]
        [TestCase(0, 0, 3, 0, 0, "Duplicated rows:")]
        [TestCase(0, 0, 0, 0, 3, "Non matching value rows:")]
        public void WriteCompared_WithSpecialRows_DisplayTextForThisKindOfRows(
            int missingRowCount
            , int unexpectedRowCount
            , int duplicatedRowCount
            , int keyMatchingRowCount
            , int nonMatchingValueRowCount
            , string expectedText)
        {
            var compared = ResultResultSet.Build(
                    GetDataRows(missingRowCount)
                    , GetDataRows(unexpectedRowCount)
                    , GetDataRows(duplicatedRowCount)
                    , GetDataRows(keyMatchingRowCount)
                    , GetDataRows(nonMatchingValueRowCount)
                );

            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new ComparisonMessengerMarkdown(EngineStyle.ByIndex, samplers);
            var writer = new StringBuilder();
            msg.WriteAnalysis(compared, writer);

            Assert.That(writer.ToString(), Does.Contain(expectedText));
        }
    }
}
