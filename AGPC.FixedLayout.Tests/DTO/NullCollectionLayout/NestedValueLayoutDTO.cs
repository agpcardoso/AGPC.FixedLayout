using AGPC.FixedLayout;

namespace AGPC.FixedLayout.Tests.DTO.NullCollectionLayout
{
    public sealed class NestedValueLayoutDTO : FixedLayout
    {
        [FieldDefinition(Length = 3)]
        public string Value { get; set; }
    }
}