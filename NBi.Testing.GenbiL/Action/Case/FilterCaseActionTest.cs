using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Action;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.GenbiL.Stateful;

namespace NBi.Testing.GenbiL.Action.Case
{
    public class FilterCaseActionTest
    {
        [Test]
        public void Display_LikeOneValue_CorrectString()
        {
            var action = new FilterCaseAction("myColumn", OperatorType.Like, new[] { "first value" }, false);
            Assert.That(action.Display, Is.EqualTo("Filtering on column 'myColumn' all instances like 'first value'"));
        }

        [Test]
        public void Display_NotLikeOneValue_CorrectString()
        {
            var action = new FilterCaseAction("myColumn", OperatorType.Like, new[] { "first value" }, true);
            Assert.That(action.Display, Is.EqualTo("Filtering on column 'myColumn' all instances not like 'first value'"));
        }

        [Test]
        public void Display_EqualOneValue_CorrectString()
        {
            var action = new FilterCaseAction("myColumn", OperatorType.Equal, new[] { "first value" }, false);
            Assert.That(action.Display, Is.EqualTo("Filtering on column 'myColumn' all instances equal to 'first value'"));
        }

        [Test]
        public void Display_LikeMultipleValues_CorrectString()
        {
            var action = new FilterCaseAction("myColumn", OperatorType.Like, new[] { "first value", "second value" }, false);
            Assert.That(action.Display, Is.EqualTo("Filtering on column 'myColumn' all instances like 'first value', 'second value'"));
        }

        [Test]
        public void Display_EqualEmpty_CorrectString()
        {
            var action = new FilterCaseAction("myColumn", OperatorType.Equal, new[] { "" }, false);
            Assert.That(action.Display, Is.EqualTo("Filtering on column 'myColumn' all instances equal to ''"));
        }

        [Test]
        public void Execute_EqualEmpty_OnlyEmptyRowsRemaining()
        {
            var state = new GenerationState();
            state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
            var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "";
            state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.CaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "firstCell2";
            secondRow[1] = "secondCell2";
            state.CaseCollection.CurrentScope.Content.Rows.Add(secondRow);

            var action = new FilterCaseAction("secondColumn", OperatorType.Equal, new[] { "" }, false);
            action.Execute(state);
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[0], Is.EqualTo("firstCell1"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Is.EqualTo(""));
        }

        [Test]
        public void Execute_EqualExplicitEmpty_OnlyEmptyRowsRemaining()
        {
            var state = new GenerationState();
            state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
            var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "(empty)";
            state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.CaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "firstCell2";
            secondRow[1] = "secondCell2";
            state.CaseCollection.CurrentScope.Content.Rows.Add(secondRow);

            var action = new FilterCaseAction("secondColumn", OperatorType.Equal, new[] { "(empty)" }, false);
            action.Execute(state);
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[0], Is.EqualTo("firstCell1"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Is.EqualTo("(empty)"));
        }

        [Test]
        public void Execute_EqualNone_OnlyNoneRowsRemaining()
        {
            var state = new GenerationState();
            state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
            var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "";
            state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.CaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "firstCell2";
            secondRow[1] = "(none)";
            state.CaseCollection.CurrentScope.Content.Rows.Add(secondRow);

            var action = new FilterCaseAction("secondColumn", OperatorType.Equal, new[] { "(none)" }, false);
            action.Execute(state);
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[0], Is.EqualTo("firstCell2"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0].ItemArray[1], Is.EqualTo("(none)"));
        }
    }
}
