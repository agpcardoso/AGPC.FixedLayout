[![RepositoryViews](https://komarev.com/ghpvc/?username=agpcardoso&color=brightgreen&style=for-the-badge)](https://github.com/antonkomarev/github-profile-views-counter)

# AGPC.FixedLayout

Get the Nuget package from <a href="https://www.nuget.org/packages/AGPC.FixedLayout">NuGet.org</a>.

This library helps you integrate .NET applications with systems that use fixed-width text records. Mainframe integrations are one example. It builds and parses fixed-width strings based on the layout defined in your DTOs, so you do not have to struggle with padding and substring operations.



## Version 1.0.1 — What's New

Version 1.0.1 adds a native `net9.0` target while retaining `netstandard2.0` for existing consumers. No public API signatures were changed.

- **Improved performance:** Layout metadata is cached by DTO type, and serialization writes directly to a `StringBuilder` to reduce repeated reflection and temporary string allocations.
- **Null collection handling:** During `ToConcatString()`, a null collection marked with `[FieldObjType]` is treated as empty. A null single-object property still throws an exception to prevent subsequent fields from shifting.
- **Concurrent metadata access:** The metadata cache supports concurrent access. DTO instances should not be modified while they are being serialized.

### Performance comparison

This local Release benchmark serialized a `ReportOrdersDTO` containing 300,000 orders. Both versions produced the same 54,900,058-character output, confirmed by matching SHA-256 hashes.

| Version | Median serialization time | Bytes allocated on the calling thread |
| --- | ---: | ---: |
| Nuget 1.0.0 | 6,199 ms | 4,714,653,680 bytes |
| Version 1.0.1 source | 296 ms | 291,293,992 bytes |

The current source was about 20.9 times faster and allocated about 93.8% fewer bytes in this test.

Each version was warmed with 100 records. We then measured five interleaved runs and report the median. Allocated bytes are cumulative allocations during serialization, not peak memory usage. Results are specific to this machine and are not a performance guarantee.

**Test environment**

- Laptop: Samsung 550XCJ/550XCR
- CPU: Intel Core i5-10210U, 4 cores and 8 logical processors
- Memory: 23.8 GB reported by Windows
- Operating system: Windows 11 Pro 64-bit
- .NET: SDK 9.0.200 and runtime 9.0.2
- Build configuration: Release

# How to use the library

### Simple Object

Some integrations use a single long string instead of XML or JSON. Each section of the string contains the value of a specific field. 

For example:

Suppose you need to send product data to a legacy system. Its integration documentation contains the following layout:

```
Field Name            Length            Start Position
------------------------------------------------------
ProductName             50                    01         
Price                   10                    51
IdCategory              05                    61
CategoryDescription     25                    66
```

Based on this layout, the product record must be sent as the following string:
```
"Name of Product                                   13,5      51   Category Description     "
````
Without a library, we would have to retrieve each field value and handle padding, substrings, and other formatting ourselves. If the layout changed, we would also need to update that logic.

I created this library after repeatedly encountering integrations like this.

To create the concatenated string shown above with AGPC.FixedLayout, follow these steps:

#### Add the <a href="https://www.nuget.org/packages/AGPC.FixedLayout">Nuget package</a> to your project

#### Create a DTO class like this one

```C#

using AGPC.FixedLayout;

namespace MyProjectTest
{
    public class ProductDTO : FixedLayout
    {
        [FieldDefinition(Length = 50)]
        public string Name{get;set;}

        [FieldDefinition(Length = 10)]
        public decimal Price { get; set; }

        [FieldDefinition(Length = 5)]
        public int IdCategory { get; set; }

        [FieldDefinition(Length = 25)]
        public string CategoryDescription { get; set; }
    }
}

```
First, add the following using directive to access the package's classes and methods:

```C#
using AGPC.FixedLayout;
```

Your DTO class should inherit from FixedLayout. This gives it access to the ToConcatString() and ToLoadThisObject() methods, which are explained below.

```C#
public class ProductDTO : FixedLayout
```
Use the FieldDefinition attribute to specify the layout settings for a DTO property. In the example below, the Name field is 50 characters long. Fields appear in the same order as their properties in the DTO. 

In this example, the first field in ProductDTO is Name, the second is Price, and so on. 
```C#
[FieldDefinition(Length = 50)]
public string Name{get;set;}
```
#### The ToConcatString() method

ToConcatString() reads the DTO property values and builds a concatenated string according to the layout settings defined on the class.
```C#
string _anyVariable = _dtoObjectVariable.ToConcatString();
```

Here is a complete example that creates a concatenated string from the DTO property values: 

```C#
var _prd = new ProductDTO();

_prd.Name = "Name of Product";
_prd.Price = 13.5M;
_prd.IdCategory = 51;
_prd.CategoryDescription = "Category Description";

var _actual = _prd.ToConcatString();

Debug.WriteLine(_actual);

```
Output:
```
"Name of Product                                   13,5      51   Category Description     "
```
#### The ToLoadThisObject() method

ToLoadThisObject() parses the string passed as a parameter and loads its values into the DTO properties. Use it to map a concatenated string back to a DTO, as shown below.

```C#
string _concatedValue = "Name of Product                                   13,5      51   Category Description     ";

var _prd = new ProductDTO();
_prd.ToLoadThisObject(_concatedValue);
```



### Custom field formatting

Suppose another system requires dates in `yyyyMMdd` format and monetary values padded with leading zeros and no decimal separator. For example, `$150.00` could be represented as `000000000015000`.

The following example shows one way to do this:

#### Requirements
```
Field Name            Length            Start Position              Format
--------------------------------------------------------------------------------
StartDate               08                    01                YYYYMMDD
EstimatedTotalSale      15                    09                000000000000000
SaleName                20                    24
```
#### Example

```C#

// Apply fixed-layout settings to the DTO
//------------------------------------

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


// Create an example entity
//-------------------------------------------
var _2019_saleEntity = Domain.Entity.SaleEntity.Create("Black Friday 2019", new DateTime(2019, 11, 29), 804259.99M);

// Assign entity values to the DTO using the required formats
//------------------------------------------------------------

var _saleDTO = new SaleDTO();
_saleDTO.SaleName = _2019_saleEntity.SaleName;
_saleDTO.EstimatedTotalSale = (_2019_saleEntity.EstimatedTotalSale * 100).ToString("000000000000000");
_saleDTO.StartDateYear = _2019_saleEntity.StartDate.ToString("yyyy");
_saleDTO.StartDateMonth = _2019_saleEntity.StartDate.ToString("MM");
_saleDTO.StartDateDay = _2019_saleEntity.StartDate.ToString("dd");


var _concatenatedString2019 = _saleDTO.ToConcatString();

Debug.WriteLine(_concatenatedString2019);

```
Output:
```
"20191129000000080425999Black Friday 2019   "
```

To map the concatenated string back to a SaleDTO object, call ToLoadThisObject():

```C#
string _2019_concatenatedstring = "20191129000000080425999Black Friday 2019   ";

var _saleDTO2019 = new SaleDTO();

_saleDTO2019.ToLoadThisObject(_2019_concatenatedstring);
 
// Read the values loaded into the DTO
//-----------------------------------------------------
 
string _saleName = _saleDTO2019.SaleName; 

DateTime _startDate =   new DateTime(Convert.ToInt16(_saleDTO2019.StartDateYear), 
                            Convert.ToInt16(_saleDTO2019.StartDateMonth), 
                            Convert.ToInt16(_saleDTO2019.StartDateDay));

decimal _estimatedTotalSale = Convert.ToDecimal(_saleDTO2019.EstimatedTotalSale) / 100);

```



### Choosing which side to pad

By default, if a value is shorter than its field length, the remaining space is filled with whitespace on the right. You can change the padding side by setting the WhiteSpaces property, as shown below:

#### Left padding
```C#
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
    
```

```C#

var _prd = new DTO.WithWhiteSpaces.ProductLeftWhiteSpacesDTO();

_prd.Name = "Name of Product";
_prd.Price = 13.5M;
_prd.IdCategory = 51;
_prd.CategoryDescription = "Category Description";


var _actual = _prd.ToConcatString();

Debug.WriteLine(_actual);

```
Output:
```
"                                   Name of Product      13,5   51     Category Description"
```

#### Left and right padding
```C#
public class ProductLeftAndRightWhiteSpacesDTO : FixedLayout
{
    [FieldDefinition(Length = 50, WhiteSpaces = FieldDefinition.Side.Left)]
    public string Name { get; set; }

    [FieldDefinition(Length = 10, WhiteSpaces = FieldDefinition.Side.Right)]
    public decimal Price { get; set; }

    [FieldDefinition(Length = 5, WhiteSpaces = FieldDefinition.Side.Left)]
    public int IdCategory { get; set; }

    [FieldDefinition(Length = 25, WhiteSpaces = FieldDefinition.Side.Right)]
    public string CategoryDescription { get; set; }
}
```
```C#
var _prd = new DTO.WithWhiteSpaces.ProductLeftAndRightWhiteSpacesDTO();

_prd.Name = "Name of Product";
_prd.Price = 13.5M;
_prd.IdCategory = 51;
_prd.CategoryDescription = "Category Description";

var _actual = _prd.ToConcatString();

Debug.WriteLine(_actual);

```
Output:
```
"                                   Name of Product13,5         51Category Description     "
```


### Adding a delimiter between fields

Some integrations require a delimiter between fields. Set the FieldDelimiter attribute to the required string:

```C#
[FieldDelimiter(";")]
public class ProductLeftWhiteSpacesDTO : FixedLayout
..

[FieldDelimiter("-->")]
public class ProductRightWhiteSpacesDTO : FixedLayout
..
```
The following examples show how to add a delimiter between DTO property values.

#### Example 1: semicolon delimiter
```C#
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

```
```C#
var _prd = new DTO.WithWhiteSpaces.WithDelimiter.ProductLeftWhiteSpacesDTO();

_prd.Name = "Name of Product";
_prd.Price = 13.5M;
_prd.IdCategory = 51;
_prd.CategoryDescription = "Category Description";

var _actual = _prd.ToConcatString();

Debug.WriteLine(_actual);
```
Output:
```C#
"                                   Name of Product;      13,5;   51;     Category Description;"
```

#### Example 2: arrow delimiter

```C#
    [FieldDelimiter("-->")]
    public class ProductRightWhiteSpacesDTO : FixedLayout
    {
        [FieldDefinition(Length = 50, WhiteSpaces = FieldDefinition.Side.Right)]
        public string Name { get; set; }

        [FieldDefinition(Length = 10, WhiteSpaces = FieldDefinition.Side.Right)]
        public decimal Price { get; set; }

        [FieldDefinition(Length = 5, WhiteSpaces = FieldDefinition.Side.Right)]
        public int IdCategory { get; set; }

        [FieldDefinition(Length = 25, WhiteSpaces = FieldDefinition.Side.Right)]
        public string CategoryDescription { get; set; }
    }
```
```C#
var _prd = new DTO.WithWhiteSpaces.WithDelimiter.ProductRightWhiteSpacesDTO();

_prd.Name = "Name of Product";
_prd.Price = 13.5M;
_prd.IdCategory = 51;
_prd.CategoryDescription = "Category Description";


var _actual = _prd.ToConcatString();

Debug.WriteLine(_actual);
```
Output:
```C#
"Name of Product                                   -->13,5      -->51   -->Category Description     -->"
```

### Mapping DTOs with object properties

The following example shows how to include another DTO through an object property.

#### Example

Main DTO object
```C#
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
    
```

Notice the [FieldObjType] attribute on the Product property:
```C#
    [FieldObjType]
    public ProductDTO Product { get; set; } = new ProductDTO();
```

Product is an object property, so it must be marked with [FieldObjType].


```C#
    var _order = new DTO.OrderDTO();
    string _expectedDate = DateTime.Now.ToShortDateString();

    _order.OrderId = 123;
    _order.CreateDate = DateTime.Now;
    _order.Notes = "Any note";

    var _prd1 = new DTO.ProductDTO();
    _prd1.Name = "Product number 1";
    _prd1.Price = 1.12M;
    _prd1.IdCategory = 51;
    _prd1.CategoryDescription = "Category Description 1";

    _order.Product = _prd1;

    var _actual = _order.ToConcatString();

    Debug.WriteLine(_actual);
```
Output:
```C#
"123     19/06/2020Product number 1                                  1,12      51   Category Description 1   Any note                                                                   "
```

#### Parsing the string back into an object

You can also map a concatenated string back to an object with an object property, as long as the layout is defined correctly.

```C#
var _actual = new DTO.OrderDTO();
string _concatedValue = "123     31/08/2019Product number 1                                  1,12      51   Category Description 1   Any note                                                                   ";

_actual.ToLoadThisObject(_concatedValue);
```


### Mapping DTOs with IEnumerable properties

To include objects from an IEnumerable property in a concatenated string, mark the property with [FieldObjType].

#### Example

DTO classes
```C#
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
```

Example
```C#
    var _reportOrdersDTO = new ReportOrdersDTO();
    var _ordersList = new List<OrderDTO>();
    var _product_1_DTO = new ProductDTO();
    var _order_1_DTO = new OrderDTO();
    var _product_2_DTO = new ProductDTO();
    var _order_2_DTO = new OrderDTO();
    var _product_3_DTO = new ProductDTO();
    var _order_3_DTO = new OrderDTO();

    _reportOrdersDTO.Name = "Report's Name";
    _reportOrdersDTO.Sponsor = "Alexandre Cardoso";

    _order_1_DTO.OrderId = 2887;
    _order_1_DTO.CreateDate = Convert.ToDateTime("25/05/2019");
    _product_1_DTO.Name = "Rice 1kg";
    _product_1_DTO.IdCategory = 298;
    _product_1_DTO.CategoryDescription = "Foods";
    _product_1_DTO.Price = 9.85M;
    _order_1_DTO.Product = _product_1_DTO;
    _order_1_DTO.Notes = "Order to delivery";

    _ordersList.Add(_order_1_DTO);

    _order_2_DTO.OrderId = 29877;
    _order_2_DTO.CreateDate = Convert.ToDateTime("24/05/2019");
    _product_2_DTO.Name = "Protex Soap";
    _product_2_DTO.IdCategory = 296;
    _product_2_DTO.CategoryDescription = "Body Wash";
    _product_2_DTO.Price = 3.45M;
    _order_2_DTO.Product = _product_2_DTO;
    _order_2_DTO.Notes = "Some Note";


    _ordersList.Add(_order_2_DTO);

    _order_3_DTO.OrderId = 2887;
    _order_3_DTO.CreateDate = Convert.ToDateTime("23/05/2019");
    _product_3_DTO.Name = "Beer";
    _product_3_DTO.IdCategory = 298;
    _product_3_DTO.CategoryDescription = "Drinks";
    _product_3_DTO.Price = 9.85M;
    _order_3_DTO.Product = _product_3_DTO;
    _order_3_DTO.Notes = "Order to delivery";

    _ordersList.Add(_order_3_DTO);

    _reportOrdersDTO.Code = 8728;
    _reportOrdersDTO.CreateDate = Convert.ToDateTime("21/09/2019");

    _reportOrdersDTO.OrderList = _ordersList;


    var _actual = _reportOrdersDTO.ToConcatString();
    
    Debug.WriteLine(_actual);
```

Output:
```C#
"Report's Name  Alexandre Cardoso        2887    25/05/2019Rice 1kg                                          9,85      298  Foods                    Order to delivery                                                          29877   24/05/2019Protex Soap                                       3,45      296  Body Wash                Some Note                                                                  2887    23/05/2019Beer                                              9,85      298  Drinks                   Order to delivery                                                          8728    21/09/2019"

```

#### Parsing the string back into an object

You can also map a concatenated string back into an object with an IEnumerable property, as long as the layout is defined correctly.


Create the DTO instance:
```C#
var _actual = new DTO.ReportOrdersDTO();
```
Create a list with one DTO instance for each expected occurrence. This example parses a concatenated string containing three orders.
```C#
List<OrderDTO> _orderList = new List<OrderDTO>();
_orderList.Add(new OrderDTO());
_orderList.Add(new OrderDTO());
_orderList.Add(new OrderDTO());
_actual.OrderList = _orderList;
```

Next, store the string that you want to map to the object: 
```C#
string _concatedValue = "Report's Name  Alexandre Cardoso        2887    25/05/2019Rice 1kg                                          9,85      298  Foods                    Order to delivery                                                          29877   24/05/2019Protex Soap                                       3,45      296  Body Wash                Some Note                                                                  2887    23/05/2019Beer                                              9,85      298  Drinks                   Order to delivery                                                          8728    21/09/2019";
```

Finally, map the contents of _concatedValue to the _actual object:

```C#
_actual.ToLoadThisObject(_concatedValue);
```

You can then read each value from the _actual instance.
