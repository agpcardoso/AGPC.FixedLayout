using System;
using System.Collections.Generic;
using System.Text;

namespace AGPC.FixedLayout.Tests.DTO.WithWhiteSpaces.WithDelimiter
{
    [FieldDelimiter(";")]
    public class ProductLeftWhiteSpacesDTO : FixedLayout
    {
        [FieldDefinition(Length = 50, WhiteSpaces = FieldDefinition.Side.Left)]
        public string Name { get; set; }

        [FieldDefinition(Length = 10, WhiteSpaces = FieldDefinition.Side.Left)]
        public decimal Price { get; set; }

        [FieldDefinition(Length = 5, WhiteSpaces = FieldDefinition.Side.Left)]
        public int IdCategory { get; set; }

        [FieldDefinition(Length = 25, WhiteSpaces = FieldDefinition.Side.Left)]
        public string CategoryDescription { get; set; }
        
    }
}
