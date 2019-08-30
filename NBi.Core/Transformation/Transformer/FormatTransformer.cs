﻿using Microsoft.CSharp;
using NBi.Core.Scalar.Casting;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Transformation.Transformer
{
    class FormatTransformer<T> : ITransformer
    {
        private string method;

        public FormatTransformer()
        {
        }

        public void Initialize(string code)
        {
           method = "{0:" + code + "}";
        }

        public object Execute(object value)
        {
            if (method == null)
                throw new InvalidOperationException();

            var factory = new CasterFactory<T>();
            var caster = factory.Instantiate();
            var typedValue = caster.Execute(value);

            var transformedValue = String.Format(method, typedValue);

            return transformedValue;
        }
    }
}
