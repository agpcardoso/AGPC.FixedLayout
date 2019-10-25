using System;
using System.Collections.Generic;
using System.Text;

namespace AGPC.FixedLayout.Tests.DTO
{
    [FieldDelimiter("	")]
    public class ProductWithDelimiterDTO : FixedLayout
    {
        [FieldDefinition(Length = 50)]
        public string Name { get; set; }

        [FieldDefinition(Length = 10)]
        public decimal Price { get; set; }

        [FieldDefinition(Length = 5)]
        public int IdCategory { get; set; }

        [FieldDefinition(Length = 25)]
        public string CategoryDescription { get; set; }
    }
}
