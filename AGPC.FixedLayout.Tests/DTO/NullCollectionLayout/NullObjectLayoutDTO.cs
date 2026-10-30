using AGPC.FixedLayout;

namespace AGPC.FixedLayout.Tests.DTO.NullCollectionLayout
{
    public sealed class NullObjectLayoutDTO : FixedLayout
    {
        [FieldDefinition(Length = 4)]
        public string Prefix { get; set; }

        [FieldObjType]
        public NestedValueLayoutDTO Item { get; set; }

        [FieldDefinition(Length = 4)]
        public string Suffix { get; set; }
    }
}