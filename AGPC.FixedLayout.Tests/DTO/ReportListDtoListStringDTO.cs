using System;
using System.Collections.Generic;
using System.Text;

namespace AGPC.FixedLayout.Tests.DTO
{
    public class ReportListDtoListStringDTO : FixedLayout
    {
        
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();

        public DateTime CreateDate { get; set; }

        public List<string> NotesList { get; set; } = new List<string>();

        public string CompanyName { get; set; }

    }
}
