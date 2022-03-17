namespace AGPC.FixedLayout.Tests.DTO.WithPaddingChar.WithDelimiter
{
    [FieldDelimiter(";")]
    public class ProductPaddingCharLeftWhiteSpacesDTO : FixedLayout
    {
        [FieldDefinition(Length = 50, WhiteSpaces = FieldDefinition.Side.Left)]
        public string Name { get; set; }

        [FieldDefinition(Length = 10, WhiteSpaces = FieldDefinition.Side.Left, PaddingChar = '0')]
        public decimal Price { get; set; }

        [FieldDefinition(Length = 5, WhiteSpaces = FieldDefinition.Side.Left, PaddingChar = '1')]
        public int IdCategory { get; set; }

        [FieldDefinition(Length = 25, WhiteSpaces = FieldDefinition.Side.Left)]
        public string CategoryDescription { get; set; }
        
    }
}