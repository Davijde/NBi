﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public class OverridenVariable : ITestVariable
    {
        private string Name { get; set; }
        private object Value { get; set; }

        public OverridenVariable(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public object GetValue() => Value;

        public bool IsEvaluated() => true;
    }
}
