﻿using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Acceptance.CustomCode
{
    public class CustomVariableDaysBetween : IScalarResolver
    {
        private DateTime From { get; }
        private DateTime To { get; }

        public CustomVariableDaysBetween(DateTime from, DateTime to)
            => (From, To) = (from, to);

        public object Execute() => To.Subtract(From).TotalDays;
    }
}
