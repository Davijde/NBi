﻿using NBi.Core;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Framework.FailureMessage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Extensibility.Resolving;
using NBi.Extensibility;

namespace NBi.NUnit.ResultSetComparison
{
    public class LookupReverseExistsConstraint : LookupExistsConstraint
    {
        public LookupReverseExistsConstraint(IResultSetResolver reference)
            : base(reference)
        { }

        public new LookupReverseExistsConstraint Using(ColumnMappingCollection mappings)
            => base.Using(mappings) as LookupReverseExistsConstraint;

        public override bool ProcessParallel(IResultSetResolver actual)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, string.Format("Queries exectued in parallel."));

            Parallel.Invoke(
                () => { rsReference = actual.Execute(); },
                () => { rsCandidate = referenceResolver.Execute(); }
            );

        //    return Matches(rsReference);
        //}

        protected override bool doMatch(IResultSet actual)
        {
            violations = Engine.Execute(rsCandidate, actual);
            var output = violations.Count() == 0;

        //    if (output && Configuration?.FailureReportProfile.Mode == FailureReportMode.Always)
        //        Assert.Pass(Failure.RenderMessage());

        //    return output;
        //}

        protected override ILookupViolationMessageFormatter BuildFailure()
        {
            var factory = new LookupReverseExistsViolationsMessageFormatterFactory();
            var msg = factory.Instantiate(Configuration.FailureReportProfile);
            msg.Generate(rsReference.Rows, rsCandidate.Rows, violations, mappings, null);
            return msg;
        }
    }
}
