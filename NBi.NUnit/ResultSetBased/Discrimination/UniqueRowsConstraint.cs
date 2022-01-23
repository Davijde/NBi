using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework.Constraints;
using NBi.Extensibility;
using NBi.Core.ResultSet.Discrimination;
using NBi.Extensibility.Resolving;

namespace NBi.NUnit.ResultSetBased.Discrimination
{
    public class UniqueRowsConstraint : NBiConstraint
    {
        protected IResultSet actualResultSet;

        protected UniquenessEngine Engine { get; private set; }

        public UniqueRowsConstraint()
        {
            Engine = new OrdinalUniquenessEngine();
        }

        public UniqueRowsConstraint Using(UniquenessEngine engine)
        {
            Engine = engine;
            return this;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            switch (actual)
            {
                case IResultSetResolver x: return Matches(x);
                default: throw new ArgumentException();
            }
        }

        protected ConstraintResult Matches(IResultSetResolver actual)
        {
            var actualRs = actual.Execute();
            var result = Engine.Execute(actualRs);
            return new UniqueRowsConstraintResult(this, actualRs, result);
        }


        /// <summary>
        /// Handle a IResultSetService execute it and check if the result contains unique rows or not
        /// </summary>
        /// <param name="actual">An IResultSetService or a result-set</param>
        /// <returns>true, if the result-set has unique rows</returns>
        //public override bool Matches(object actual)
        //{
        //    if (actual is IResultSetResolver)
        //    {
        //        return Matches((actual as IResultSetResolver).Execute());
        //    }
        //    else if (actual is IResultSet)
        //    {
        //        actualResultSet = (IResultSet)actual;
        //        var result = Engine.Execute(actualResultSet);

        //        if (!result.AreUnique || Configuration.FailureReportProfile.Mode == FailureReportMode.Always)
        //        {
        //            var factory = new DataRowsMessageFormatterFactory();
        //            failure = factory.Instantiate(Configuration.FailureReportProfile, Engine is OrdinalEvaluator ? EngineStyle.ByIndex : EngineStyle.ByName);
        //            failure.BuildDuplication(actualResultSet.Rows, result);
        //        }

        //        if (result.AreUnique && Configuration?.FailureReportProfile.Mode == FailureReportMode.Always)
        //            Assert.Pass(failure.RenderMessage());

        //        return result.AreUnique;
        //    }
        //    else
        //        throw new ArgumentException();

        //}

        //#region "Error report"

        //public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        //{
        //    if (Configuration?.FailureReportProfile.Format == FailureReportFormat.Json)
        //        return;

        //    writer.WriteLine("No duplicated row.");
        //}

        //public override void WriteActualValueTo(NUnitCtr.MessageWriter writer)
        //{
        //    if (Configuration?.FailureReportProfile.Format == FailureReportFormat.Json)
        //        return;

        //    writer.WriteLine(failure.RenderActual());
        //}

        //public override void WriteMessageTo(NUnitCtr.MessageWriter writer)
        //{
        //    if (Configuration?.FailureReportProfile.Format == FailureReportFormat.Json)
        //        writer.Write(failure.RenderMessage());
        //    else
        //    {
        //        writer.WritePredicate("Execution of the query returns duplicated rows");
        //        writer.WriteLine();
        //        writer.WriteLine();
        //        base.WriteMessageTo(writer);
        //        writer.WriteLine();
        //        writer.WriteLine(failure.RenderAnalysis());
        //    }
        //}

        //#endregion

    }
}
