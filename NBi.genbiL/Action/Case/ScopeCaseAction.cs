﻿using System;
using System.Linq;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Action.Case
{
    public class ScopeCaseAction : ICaseAction
    {
        public string Name { get; set; }
        
        public ScopeCaseAction(string name)
        {
            Name = name;
        }
        public void Execute(GenerationState state)
        {
            state.TestCaseCollection.SetFocus(Name);
        }

        public virtual string Display
        {
            get
            {
                return string.Format("Focussing on test cases set named '{0}'", Name);
            }
        }
    }
}
