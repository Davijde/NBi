﻿using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.StringTemplate;
using NBi.Extensibility.Query;

namespace NBi.Core
{
    public class StringTemplateEngine : Extensibility.ITemplateEngine
    {
        public StringTemplateEngine()
        { }

        public string Render(string template, IEnumerable<KeyValuePair<string, object>> variables)
        {
            var stringTemplate = new Template(template, '$', '$');

            foreach (var variable in variables)
                stringTemplate.Add(variable.Key, variable.Value);

            var str = stringTemplate.Render();

            return str;
        }
    }
}
