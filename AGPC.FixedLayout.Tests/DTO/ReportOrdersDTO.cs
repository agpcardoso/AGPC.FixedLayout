using System;
using System.Collections.Generic;
using System.Text;

namespace AGPC.FixedLayout.Tests.DTO
{
    public class ReportOrdersDTO : FixedLayout
    {
        [FieldDefinition(Length = 15)]
        public string Name { get; set;}
        [FieldDefinition(Length = 25)]
        public string Sponsor { get; set; }
        
        [FieldObjType]
        public IEnumerable<OrderDTO> OrderList { get; set; } = new List<OrderDTO>();
        
        [FieldDefinition(Length = 8)]
        public int Code { get; set; }
        [FieldDefinition(Length =10)]
        public DateTime CreateDate { get; set; }
    }
}
