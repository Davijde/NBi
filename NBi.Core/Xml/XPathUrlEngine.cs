﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NBi.Core.Xml
{
    public class XPathUrlEngine : XPathEngine
    {
        public string Url { get; private set; }

        public XPathUrlEngine(string url, string from, IEnumerable<AbstractSelect> selects, string defaultNamespacePrefix)
            : base(from, selects, defaultNamespacePrefix)
        {
            this.Url = url;
        }

        public override IEnumerable<object> Execute()
        {
            var doc = XDocument.Load(Url);
            return Execute(doc);
        }
    }
}
