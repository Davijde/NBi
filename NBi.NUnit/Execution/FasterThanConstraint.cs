﻿using System;
using NBi.Extensibility;
using NBi.Extensibility.Decoration.DataEngineering;
using NUnit.Framework.Constraints;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Execution
{
    public class FasterThanConstraint : NBiConstraint
    {
        /// <summary>
        /// Store for the result of the engine's execution
        /// </summary>
        protected IExecutionResult Result;

        protected int maxTimeMilliSeconds;
        protected int timeOutMilliSeconds;
        protected bool cleanCache;

        public FasterThanConstraint()
        {

        }

        public FasterThanConstraint MaxTimeMilliSeconds(int value)
        {
            this.maxTimeMilliSeconds = value;
            return this;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handle a sql string or a sqlCommand and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string or SQL Command</param>
        /// <returns>true, if the query defined in parameter is executed in less that expected else false</returns>
        //public override bool Matches(object actual)
        //{
        //    if (actual is IExecutable)
        //        return doMatch((IExecutable)actual);
        //    else
        //        return false;
        //}

        /// <summary>
        /// Handle a sql string and check it with the engine
        /// </summary>
        /// <param name="actual">SQL string</param>
        /// <returns>true, if the query defined in parameter is executed in less that expected else false</returns>
        //public bool doMatch(IExecutable actual)
        //{
        //    Result = actual.Execute();
        //    return 
        //        (
        //            Result.TimeElapsed.TotalMilliseconds < maxTimeMilliSeconds
        //        );
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="writer"></param>
        //public override void WriteDescriptionTo(NUnitCtr.MessageWriter writer)
        //{
        //    var sb = new System.Text.StringBuilder();
        //    sb.AppendLine("Execution of the query is slower than expected");
        //    sb.AppendFormat("Maximum expected was {0}ms", maxTimeMilliSeconds);
        //    writer.WritePredicate(sb.ToString());           
        //}

        //public override void  WriteActualValueTo(NUnitCtr.MessageWriter writer)
        //{
        //    writer.WriteActualValue(string.Format("{0}ms", Result.TimeElapsed.TotalMilliseconds));
        //}
    }
}