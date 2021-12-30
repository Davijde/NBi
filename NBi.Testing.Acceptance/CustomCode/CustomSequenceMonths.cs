﻿using NBi.Extensibility.Resolving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Acceptance.CustomCode
{
    public class CustomSequenceMonths : ISequenceResolver
    {
        public CustomSequenceMonths()
        { }

        object IResolver.Execute() => Execute();

        public IList Execute() => new DateTime[] 
        { 
            new DateTime(2016, 1, 1), 
            new DateTime(2016, 2, 1), 
            new DateTime(2016, 3, 1) 
        };
    }
}
