using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSPOS.Client
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PlugDisplayNameAttribute : Attribute
    {
        private string _displayName;

        public PlugDisplayNameAttribute(string DisplayName)
            : base()
        {
            _displayName = DisplayName;
            return;
        }

        public override string ToString()
        {
            return _displayName;
        }
         
    }
}
