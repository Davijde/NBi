﻿using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using NBi.GenbiL.Stateful;
using NBi.Xml.Settings;

namespace NBi.GenbiL.Action.Setting
{
    public class CsvProfileEmptyCellAction : ISettingAction
    {
        string Value { get; set; }

        public CsvProfileEmptyCellAction(string value)
        {
            Value  = value;
        }

        public void Execute(GenerationState state)
        {
            var newProfile = state.Settings.CsvProfile;
            if (newProfile == null)
            {
                newProfile = new CsvProfileXml() { };
                state.Settings.CsvProfile = (newProfile);
            }
            newProfile.EmptyCell = Value;
        }

        public string Display => $"Create CSV profile setting named 'EmptyCell' and defining it to '{Value}'";
    }
}
