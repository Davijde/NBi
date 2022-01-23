﻿using System;
using System.Linq;
using NBi.Core.ResultSet.Filtering;
using NUnit.Framework.Constraints;

namespace NBi.NUnit.ResultSetBased.RowPredicate
{
    public class AllRowsConstraint : RowCountFilterConstraint
    {
        public AllRowsConstraint(IPredicateFilter filter)
            : base(new EqualConstraint(0), filter) 
        {
            filter.Revert();    
        }

        public override string Description
        {
            get => $"all rows validating the predicate '{Filter.Describe()}'";
            protected set => base.Description = value;
        }
    }
}