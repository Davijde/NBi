﻿using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet.Lookup
{
    public class KeysRetrieverByNameTest
    {
        protected DataTable BuildDataTable(object[] keys, object[] secondKeys, object[] values)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            var keyCol = dt.Columns.Add("zero");
            var secondKeyCol = dt.Columns.Add("one");
            var valueCol = dt.Columns.Add("two");

            for (int i = 0; i < keys.Length; i++)
            {
                var dr = dt.NewRow();
                dr.SetField<object>(keyCol, keys[i]);
                dr.SetField<object>(secondKeyCol, secondKeys[i]);
                dr.SetField<object>(valueCol, values[i]);
                dt.Rows.Add(dr);
            }

            return dt;
        }

        [Test]
        public void GetKeys_UniqueCell_CorrectCell()
        {
            var table = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });

            var columns = new List<IColumnDefinition>()
            {
                new Column() { Name = "zero", Type=ColumnType.Text}
            };

            var keyRetriever = new KeysRetrieverByName(columns);
            Assert.That(keyRetriever.GetKeys(table.Rows[0]).Members, Is.EqualTo(new[] { "Key0" }));
            Assert.That(keyRetriever.GetKeys(table.Rows[1]).Members, Is.EqualTo(new[] { "Key1" }));
            Assert.That(keyRetriever.GetKeys(table.Rows[2]).Members, Is.EqualTo(new[] { "Key0" }));
        }

        [Test]
        public void GetKeys_UniqueCellNumeric_CorrectCell()
        {
            var table = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });

            var columns = new List<IColumnDefinition>()
            {
                new Column() { Name = "two", Type=ColumnType.Numeric}
            };

            var keyRetriever = new KeysRetrieverByName(columns);
            Assert.That(keyRetriever.GetKeys(table.Rows[0]).Members, Is.EqualTo(new[] { 0 }));
            Assert.That(keyRetriever.GetKeys(table.Rows[1]).Members, Is.EqualTo(new[] { 1 }));
            Assert.That(keyRetriever.GetKeys(table.Rows[2]).Members, Is.EqualTo(new[] { 0 }));
        }

        [Test]
        public void GetKeys_UniqueCellNumericCasting_CorrectCell()
        {
            var table = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { "0", "1.0", "0.00" });

            var columns = new List<IColumnDefinition>()
            {
                new Column() { Name = "two", Type=ColumnType.Numeric}
            };

            var keyRetriever = new KeysRetrieverByName(columns);
            Assert.That(keyRetriever.GetKeys(table.Rows[0]).Members, Is.EqualTo(new[] { 0 }));
            Assert.That(keyRetriever.GetKeys(table.Rows[1]).Members, Is.EqualTo(new[] { 1 }));
            Assert.That(keyRetriever.GetKeys(table.Rows[2]).Members, Is.EqualTo(new[] { 0 }));
        }

        [Test]
        public void GetKeys_TwoCells_CorrectCells()
        {
            var table = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });

            var columns = new List<IColumnDefinition>()
            {
                new Column() { Name = "zero", Type=ColumnType.Text},
                new Column() { Name = "one", Type=ColumnType.Text}
            };

            var keyRetriever = new KeysRetrieverByName(columns);
            Assert.That(keyRetriever.GetKeys(table.Rows[0]).Members, Is.EqualTo(new[] { "Key0", "Foo" }));
            Assert.That(keyRetriever.GetKeys(table.Rows[1]).Members, Is.EqualTo(new[] { "Key1", "Bar" }));
            Assert.That(keyRetriever.GetKeys(table.Rows[2]).Members, Is.EqualTo(new[] { "Key0", "Foo" }));
        }

        [Test]
        public void GetKeys_TwoCellsDifferentTypes_CorrectCells()
        {
            var table = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });

            var columns = new List<IColumnDefinition>()
            {
                new Column() { Name = "zero", Type=ColumnType.Text},
                new Column() { Name = "two", Type=ColumnType.Numeric}
            };

            var keyRetriever = new KeysRetrieverByName(columns);
            Assert.That(keyRetriever.GetKeys(table.Rows[0]).Members, Is.EqualTo(new object[] { "Key0", 0 }));
            Assert.That(keyRetriever.GetKeys(table.Rows[1]).Members, Is.EqualTo(new object[] { "Key1", 1 }));
            Assert.That(keyRetriever.GetKeys(table.Rows[2]).Members, Is.EqualTo(new object[] { "Key0", 0 }));
        }

        [Test]
        public void GetKeys_TwoCellsReverseOrder_CorrectCells()
        {
            var table = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });

            var columns = new List<IColumnDefinition>()
            {
                new Column() { Name = "one", Type=ColumnType.Text},
                new Column() { Name = "zero", Type=ColumnType.Text}
            };

            var keyRetriever = new KeysRetrieverByName(columns);
            Assert.That(keyRetriever.GetKeys(table.Rows[0]).Members, Is.EqualTo(new[] { "Foo", "Key0" }));
            Assert.That(keyRetriever.GetKeys(table.Rows[1]).Members, Is.EqualTo(new[] { "Bar", "Key1" }));
            Assert.That(keyRetriever.GetKeys(table.Rows[2]).Members, Is.EqualTo(new[] { "Foo", "Key0" }));
        }

    }
}

