using System.Collections.Generic;
using AGPC.FixedLayout;

namespace AGPC.FixedLayout.Tests.DTO.NullCollectionLayout
{
    public sealed class NullCollectionLayoutDTO : FixedLayout
    {
        [FieldDefinition(Length = 4)]
        public string Prefix { get; set; }

        [FieldObjType]
        public IEnumerable<NestedValueLayoutDTO> Items { get; set; }

        [FieldDefinition(Length = 4)]
        public string Suffix { get; set; }
    }
}