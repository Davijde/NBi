﻿using NBi.Core.Sequence.Transformation.Aggregation.Strategy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Sequence.Transformation.Strategy
{
    public class FailureMissingValueStrategyTest
    {
        [Test]
        public void Execute_NoSpecialValue_NoException()
        {
            var list = new List<object>() { 1, 3, 5 };
            var strategy = new FailureMissingValueStrategyNumeric();
            Assert.DoesNotThrow(() => strategy.Execute(list));
        }

        [Test]
        public void Execute_NoSpecialValue_SameValues()
        {
            var list = new List<object>() { 1, 3, 5 };
            var strategy = new FailureMissingValueStrategyNumeric();
            var result = strategy.Execute(list);
            Assert.That(result, Has.Member(1));
            Assert.That(result, Has.Member(3));
            Assert.That(result, Has.Member(5));
        }

        [Test]
        public void Execute_Blank_Exception()
        {
            var list = new List<object>() { 1, "(blank)", 3, 5 };
            var strategy = new FailureMissingValueStrategyNumeric();
            Assert.Throws<ArgumentException>(() => strategy.Execute(list));
        }

        [Test]
        public void Execute_Null_Exception()
        {
            var list = new List<object>() { 1, 3, 5, null };
            var strategy = new FailureMissingValueStrategyNumeric();
            Assert.Throws<ArgumentException>(() => strategy.Execute(list));
        }

        [Test]
        public void Execute_NullAsText_Exception()
        {
            var list = new List<object>() { 1, 3, 5, "(null)" };
            var strategy = new FailureMissingValueStrategyNumeric();
            Assert.Throws<ArgumentException>(() => strategy.Execute(list));
        }


        [Test]
        public void Execute_TextNullAsText_Exception()
        {
            var list = new List<object>() { 1, 3, 5, "(null)" };
            var strategy = new FailureMissingValueStrategyText();
            Assert.Throws<ArgumentException>(() => strategy.Execute(list));
        }
    }
}
