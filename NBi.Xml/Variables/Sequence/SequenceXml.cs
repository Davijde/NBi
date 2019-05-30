﻿using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NBi.Xml.Variables.Sequence
{
    public class SequenceXml
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public ColumnType Type { get; set; }

        [XmlElement("loop-sentinel")]
        public SentinelLoopXml SentinelLoop { get; set; }

        [XmlElement("item")]
        public List<string> Items { get; set; } = new List<string>();

        [XmlIgnore]
        public bool ItemsSpecified { get => Items.Count > 0; set { } }
    }
}
