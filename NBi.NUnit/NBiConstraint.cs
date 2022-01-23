﻿using NBi.Core.Configuration;
using NBi.Core.Variable;
using NBi.Framework;
using NBi.NUnit.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit
{
    public abstract class NBiConstraint : NUnitCtr.Constraint
    {
        public IConfiguration Configuration {get; set;} = Core.Configuration.Configuration.Default;
        public IDictionary<string, IVariable> GlobalVariables { get; set; }

        public NBiConstraint()
        {
        }
    }
}
