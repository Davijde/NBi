﻿using NBi.Core.Sequence.Transformation.Aggregation.Strategy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Sequence.Transformation.Strategy
{
    public class DropStrategyTest
    {
        [Test]
        public void Execute_NothingToDrop_NothingDropped()
        {
            var list = new List<object>() { 1, 3, 5 };
            var strategy = new DropStrategyNumeric();
            Assert.That(strategy.Execute(list).Count, Is.EqualTo(3));
        }

        [Test]
        public void Execute_NothingToDrop_SameValues()
        {
            var list = new List<object>() { 1, 3, 5 };
            var strategy = new DropStrategyNumeric();
            Assert.That(strategy.Execute(list), Has.Member(1));
            Assert.That(strategy.Execute(list), Has.Member(3));
            Assert.That(strategy.Execute(list), Has.Member(5));
        }

        [Test]
        public void Execute_Blank_BlankDropped()
        {
            var list = new List<object>() { 1, "(blank)", 3, 5 };
            var strategy = new DropStrategyNumeric();
            Assert.That(strategy.Execute(list).Count, Is.EqualTo(3));
            Assert.That(strategy.Execute(list), Has.Member(1));
            Assert.That(strategy.Execute(list), Has.Member(3));
            Assert.That(strategy.Execute(list), Has.Member(5));
        }

        [Test]
        public void Execute_Null_NullDropped()
        {
            var list = new List<object>() { 1, 3, 5, null };
            var strategy = new DropStrategyNumeric();
            Assert.That(strategy.Execute(list).Count, Is.EqualTo(3));
            Assert.That(strategy.Execute(list), Has.Member(1));
            Assert.That(strategy.Execute(list), Has.Member(3));
            Assert.That(strategy.Execute(list), Has.Member(5));
        }

        [Test]
        public void Execute_NullAsText_NullDropped()
        {
            var list = new List<object>() { 1, "(null)", 5, null };
            var strategy = new DropStrategyNumeric();
            Assert.That(strategy.Execute(list).Count, Is.EqualTo(2));
            Assert.That(strategy.Execute(list), Has.Member(1));
            Assert.That(strategy.Execute(list), Has.Member(5));
        }

        [Test]
        public void Execute_TextNull_NullDropped()
        {
            var list = new List<object>() { "foo", "(null)", "bar", null };
            var strategy = new DropStrategyText();
            Assert.That(strategy.Execute(list).Count, Is.EqualTo(2));
            Assert.That(strategy.Execute(list), Has.Member("foo"));
            Assert.That(strategy.Execute(list), Has.Member("bar"));
        }
    }
}
