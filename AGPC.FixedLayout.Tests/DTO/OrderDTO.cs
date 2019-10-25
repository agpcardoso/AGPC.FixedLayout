using System;
using System.Collections.Generic;
using System.Text;

namespace AGPC.FixedLayout.Tests.DTO
{
    public class OrderDTO : FixedLayout
    {
        [FieldDefinition(Length = 8)]
        public int OrderId { get; set; }

        [FieldDefinition(Length = 10)]
        public DateTime CreateDate { get; set; }

        [FieldObjType]
        public ProductDTO Product { get; set; } = new ProductDTO();

        [FieldDefinition(Length = 75)]
        public string Notes { get; set; }
    }
}
