using System;
using System.Collections.Generic;
using System.Text;

namespace AGPC.FixedLayout
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class FieldDefinition : System.Attribute
    {

        public int Length { get; set; }
        public Side WhiteSpaces { get; set; }

        public enum Side
        {
            Right = 0,
            Left = 1
        }





        internal string Value { get; set; }
        internal string Name { get; set; }

        //For Allow to Map Concatenated strings To Object Properties inside other Object property
        //by reference
        internal object ObjectReferenced { get; set; }
    }
}
