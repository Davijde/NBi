using System;
using System.Data;
using System.Linq;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.Calculation;
using NBi.NUnit.Messaging;
using NUnitCtr = NUnit.Framework.Constraints;
using NBi.Framework;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet.Filtering;
using NBi.Extensibility;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class NoRowsConstraint : RowCountFilterConstraint
    {
        public NoRowsConstraint(IPredicateFilter filter)
            : base(new NUnitCtr.EqualConstraint(0), filter) { }

        public override string Description 
        { 
            get => $"no row validating the predicate '{Filter.Describe()}'"; 
            protected set => base.Description = value; 
        }
    }
}