# AGPC.FixedLayout

Nuget package <a href="https://www.nuget.org/packages/AGPC.FixedLayout">here</a>

This project will be useful if you need to do integrations based in fixed layout strings. 

An example of kind of integration that use fixed layout strings, it's some Mainframe integrations.

If you need to integrate your c# project with some mainframe api, 
probably you will do it sending and receiving strings with fixed lenghts. 

This library will help you to do it in easy way without you having 
to beating your brains out with substrings.



# Tutorial for use

### Simple Object

Some integrations, instead of using xml or json, use big strings, each string piece contains information from a field. 

For Example:

Suppose that we have to send a product data to a legacy system and the legacy integration 
documentation contains the follow instructions:

```
Field Name            Lenght            Start Position
------------------------------------------------------
ProductName             50                    01         
Price                   10                    51
IdCategory              05                    61
CategoryDescription     25                    66
```

Based in the previous layout definition, for realize the product data integration we have to send the follow string:
```
"Name of Product                                   13,5      51   Category Description     "
````
To build this string without a library for help us, we would need to get the field values, to apply some PadRights, substrings, so on, If some day, some layout field were updated, we will need to beating our brains to update all algorithm.

The Idea to build and shared this library, came up after I have often encountered integrations like it.

To create the  concatenated string mentioned earlier using AGPC.FixedLayout, we just need follow these steps:

#### Add this <a href="https://www.nuget.org/packages/AGPC.FixedLayout"> Nuget Package </a> in your project

#### Create a DTO Class like the below

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
Some points are interesting to have attention, firstly, we have to add the follow using for have access all nuget classes, methods and functions

```C#
using AGPC.FixedLayout;
```

Our DTO Class should inherit the FixedLayout class, it will give us access to ToConcatString() method and .ToLoadThisObject() method  that will be explained later

```C#
public class ProductDTO : FixedLayout
```
We use the Attribute "FieldDefinition" to decorate the properties dto class with the layout settings, bellow we set that the string created, will have 50 characters lenght respecting the order of properties disposition. 

In our example above, the first field of ProductDTO is the name, second is the price and so on. 
```C#
[FieldDefinition(Length = 50)]
public string Name{get;set;}
```
#### .ToConcatString() method

The .ToConcatString() method is responsible to get DTO properties values and build a concatenated string with all settings set in DTO class
```C#
string _anyVariable = _dtoObjectVariable.ToConcatString();
```

Finally, bellow we have a complete example for generate a concatenated string, with the DTO properties values 

```C#
var _prd = new ProductDTO();

_prd.Name = "Name of Product";
_prd.Price = 13.5M;
_prd.IdCategory = 51;
_prd.CategoryDescription = "Category Description";

var _actual = _prd.ToConcatString();

Debug.WriteLine(_actual);

```
Output Window generated string
```
"Name of Product                                   13,5      51   Category Description     "
```
#### .ToLoadThisObject() method

The .ToLoadThisObject() method is responsible to load the all caller dto properties getting and converting the string passed in  parameter, so, in this case, if you want to do the reverse, to map the concatenated string to the dto object, use the .ToLoadThis Object() method as shown bellow

```C#
string _concatedValue = "Name of Product                                   13,5      51   Category Description     ";

var _prd = new ProductDTO();
_prd.ToLoadThisObject(_concatedValue);
```



### Custom formatting values integration

Suppose that you have a need to do an integration with other system but, there are a requirement, you should integrate DateTime fields with "yyyymmdd" formatation and you should integrate monetary fields with left zeros and without decimal separators. (e. g. $150,00 = "000000000015000")

Bellow there is an example how to do it:

#### Requiriments
```
Field Name            Lenght            Start Position              Format
--------------------------------------------------------------------------------
StartDate               08                    01                YYYYMMDD
EstimatedTotalSale      15                    09                000000000000000
SaleName                20                    24
```
#### Coding

```C#

//Applying DTO fixed layout Settings
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


//Creating an example entity with some values
//-------------------------------------------
var _2019_saleEntity = Domain.Entity.SaleEntity.Create("Black Friday 2019", new DateTime(2019, 11, 29), 804259.99M);

//Attributing entity values in the DTO with custom formats
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
Output Window generated string
```
"20191129000000080425999Black Friday 2019   "
```

If you want to do the reverse, to map the concatenated string to the SaleDto object, it's simple, use the ToLoadThisObject method to do it

```C#
string _2019_concatenatedstring = "20191129000000080425999Black Friday 2019   ";

var _saleDTO2019 = new SaleDTO();

_saleDTO2019.ToLoadThisObject(_2019_concatenatedstring);
 
//Set some variables with integrated Dto Values
//-----------------------------------------------------
 
string _saleName = _saleDTO2019.SaleName; 

DateTime _startDate =   new DateTime(Convert.ToInt16(_saleDTO2019.StartDateYear), 
                            Convert.ToInt16(_saleDTO2019.StartDateMonth), 
                            Convert.ToInt16(_saleDTO2019.StartDateDay));

decimal _estimatedTotalSale = Convert.ToDecimal(_saleDTO2019.EstimatedTotalSale) / 100);

```



### Setting the white spaces side

By default, if content value not reach the max length, the remaining, will filled with white spaces in right side, 
but the filled side can be customized by setting WhiteSpaces attribute as shown bellow:

#### Left white spaces
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
Output Window generated string
```
"                                   Name of Product      13,5   51     Category Description"
```

#### Left and Right white spaces
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
Output Window generated string
```
"                                   Name of Product13,5         51Category Description     "
```


### Defining a Delimiter between each property

Some integrations have the need to include a delimiter string between each field, to do it, we will have to setting the FieldDelimiter attribute with the delimiter string required

```C#
[FieldDelimiter(";")]
public class ProductLeftWhiteSpacesDTO : FixedLayout
..

[FieldDelimiter("-->")]
public class ProductRightWhiteSpacesDTO : FixedLayout
..
```
Bellow we have two complete examples for generate a concatenated string, with each DTO property values separated by delimiter string

#### First example using string delimiter 
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
Output Window generated string
```C#
"                                   Name of Product;      13,5;   51;     Category Description;"
```

#### Second example using string delimiter 

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
Output Window generated string
```C#
"Name of Product                                   -->13,5      -->51   -->Category Description     -->"
```


This is an simple and introductory example, Coming soon I'm to describe more ways to map DTOs with Object properties and IEnumerable properties to a concatenated string and map a concatenated string to a DTO object

But...

if you can't wait, you can see some examples with more complex dtos in AGPC.Fixed Layout.Tests
