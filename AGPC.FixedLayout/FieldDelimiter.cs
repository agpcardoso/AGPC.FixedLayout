using System;
using System.Collections.Generic;
using System.Text;

namespace AGPC.FixedLayout
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class FieldDelimiter : System.Attribute
    {

        public FieldDelimiter(string delimiter)
        {
            this.Delimiter = delimiter;
        }

        internal string Delimiter {get;set;}
    }
}
