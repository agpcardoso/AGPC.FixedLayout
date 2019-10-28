using System;
using System.Collections.Generic;
using System.Text;

namespace AGPC.FixedLayout.Tests.DTO
{
    public class SaleDTO : FixedLayout
    {

        [FieldDefinition(Length = 4)]
        public string StartDateYear { get; set; }

        [FieldDefinition(Length = 2)]
        public string StartDateMonth { get; set; }
        [FieldDefinition(Length = 2)]
        public string StartDateDay { get; set; }

        [FieldDefinition(Length = 15)]
        public string EstimatedTotalSale { get; set; }

        [FieldDefinition(Length = 20)]
        public string SaleName { get; set; }

    }
}
