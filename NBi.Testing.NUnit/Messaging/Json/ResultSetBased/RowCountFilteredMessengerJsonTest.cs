﻿using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Discrimination;
using NBi.Extensibility;
using NBi.NUnit.Messaging.Json.ResultSetBased;
using NBi.Core.Sampling;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.NUnit.Messaging.Json.ResultSetBased
{
    public class RowCountFilteredMessengerJsonTest
    {
        [Test]
        public void WriteAnalysis_LessThan_Constraint()
        {
            var result = new LessThanConstraint(10).ApplyTo(15);
            var filtered = new DataTableResultSet();
            filtered.Load(new[]
            {
                new object[] { "foo", 100 }, new object[] { "foo", 120 },
                new object[] { "bar", 100 }, new object[] { "bar", 150 }
            });
            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountFilteredMessengerJson(EngineStyle.ByIndex, samplers);

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                msg.WriteAnalysis(result, filtered, writer);
                var value = sb.ToString();
                Assert.That(value, Does.Contain($"\"constraint\":{{\"type\":\"LessThan\",\"description\":\"less than 10\"}}"));
            }
        }

        [Test]
        public void WriteAnalysis_LessThan_Result()
        {
            var result = new LessThanConstraint(10).ApplyTo(15);
            var filtered = new DataTableResultSet();
            filtered.Load(new[]
            {
                new object[] { "foo", 100 }, new object[] { "foo", 120 },
                new object[] { "bar", 100 }, new object[] { "bar", 150 }
            });
            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountFilteredMessengerJson(EngineStyle.ByIndex, samplers);

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                msg.WriteAnalysis(result, filtered, writer);
                var value = sb.ToString();
                Assert.That(value, Does.Contain($"\"result\":{{\"row-count\":15,\"message\":\""));
            }
        }

        [Test]
        public void WriteAnalysis_LessThan_Predicate()
        {
            var result = new LessThanConstraint(10).ApplyTo(15);
            var filtered = new DataTableResultSet();
            filtered.Load(new[]
            {
                new object[] { "foo", 100 }, new object[] { "foo", 120 },
                new object[] { "bar", 100 }, new object[] { "bar", 150 }
            });
            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountFilteredMessengerJson(EngineStyle.ByIndex, samplers);

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                msg.WriteAnalysis(result, filtered, writer);
                var value = sb.ToString();
                Assert.That(value, Does.Contain($"\"predication-filter\":{{\"total-rows\":4,\"table\":"));
            }
        }

        [Test]
        public void WriteMessage_ActualAndChildConstraint_ActualAndAnalysis()
        {
            var result = new LessThanConstraint(10).ApplyTo(15);
            var actual = new DataTableResultSet();
            actual.Load(new[]
            {
                new object[] { "foo", 100 }, new object[] { "foo", 120 },
                new object[] { "bar", 100 }, new object[] { "bar", 150 }
            });
            var filtered = new DataTableResultSet();
            filtered.Load(new[]
            {
                new object[] { "foo", 120 }, new object[] { "bar", 150 }
            });
            var samplers = new SamplersFactory<IResultRow>().Instantiate(FailureReportProfile.Default);
            var msg = new RowCountFilteredMessengerJson(EngineStyle.ByIndex, samplers);

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                var value = msg.WriteMessage(actual, filtered, result);
                Assert.That(value, Does.Contain($"\"actual\":"));
                Assert.That(value, Does.Contain($"\"analysis\":"));
                Assert.That(value, Does.Not.Contain($"\"expected\":"));
            }
        }
    }
}