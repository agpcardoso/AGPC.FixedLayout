using System;
using Xunit;
using AGPC.FixedLayout.Tests.DTO;
using System.Collections.Generic;
using System.Linq;

namespace AGPC.FixedLayout.Tests
{
    public class FixedLayoutTests
    {

        [Fact]
        public void ToConcatString_ReturnsWithFixedLayoutDefaultSideWhiteSpaces()
        {
            ProductDTO _prd = new ProductDTO();

            _prd.Name = "Name of Product";
            _prd.Price = 13.5M;
            _prd.IdCategory = 51;
            _prd.CategoryDescription = "Category Description";


            var _actual = _prd.ToConcatString();
            var _expectedValue = "Name of Product                                   13,5      51   Category Description     ";

            Assert.Equal(_expectedValue, _actual);
        }
        [Fact]
        public void ToLoadThisObject_CreateObjBasedStringFixedLayoutDefaultSideWhiteSpa()
        {
            bool _expectedValue = true;
            var _actual = new ProductDTO();
            string _concatedValue = "Name of Product                                   13,5      51   Category Description     ";

            _actual.ToLoadThisObject(_concatedValue);

            _expectedValue = (_actual.Name == "Name of Product") &&
                (_actual.Price == 13.5M) &&
                (_actual.IdCategory == 51) &&
                (_actual.CategoryDescription == "Category Description");



            Assert.True(_expectedValue);

        }
        [Fact]
        public void ToConcatString_ReturnsWithFixedLayoutLeftWhiteSpaces()
        {
            var _prd = new DTO.WithWhiteSpaces.ProductLeftWhiteSpacesDTO();

            _prd.Name = "Name of Product";
            _prd.Price = 13.5M;
            _prd.IdCategory = 51;
            _prd.CategoryDescription = "Category Description";


            var _actual = _prd.ToConcatString();
            var _expectedValue = "                                   Name of Product      13,5   51     Category Description";

            Assert.Equal(_expectedValue, _actual);
        }
        [Fact]
        public void ToLoadThisObject_CreateObjBasedStringFixedLayoutLeftWhiteSpaces()
        {
            bool _expectedValue = true;
            var _actual = new DTO.WithWhiteSpaces.ProductLeftWhiteSpacesDTO();
            string _concatedValue = "                                   Name of Product      13,5   51     Category Description";

            _actual.ToLoadThisObject(_concatedValue);

            _expectedValue = (_actual.Name == "Name of Product") &&
                (_actual.Price == 13.5M) &&
                (_actual.IdCategory == 51) &&
                (_actual.CategoryDescription == "Category Description");



            Assert.True(_expectedValue);

        }
        [Fact]
        public void ToConcatString_ReturnsWithFixedLayoutRightWhiteSpaces()
        {
            var _prd = new DTO.WithWhiteSpaces.ProductRightWhiteSpacesDTO();

            _prd.Name = "Name of Product";
            _prd.Price = 13.5M;
            _prd.IdCategory = 51;
            _prd.CategoryDescription = "Category Description";


            var _actual = _prd.ToConcatString();
            var _expectedValue = "Name of Product                                   13,5      51   Category Description     ";


            Assert.Equal(_expectedValue, _actual);
        }
        [Fact]
        public void ToLoadThisObject_CreateObjBasedStringFixedLayoutRightWhiteSpaces()
        {
            bool _expectedValue = true;
            var _actual = new DTO.WithWhiteSpaces.ProductRightWhiteSpacesDTO();
            string _concatedValue = "Name of Product                                   13,5      51   Category Description     ";

            _actual.ToLoadThisObject(_concatedValue);

            _expectedValue = (_actual.Name == "Name of Product") &&
                (_actual.Price == 13.5M) &&
                (_actual.IdCategory == 51) &&
                (_actual.CategoryDescription == "Category Description");



            Assert.True(_expectedValue);

        }
        [Fact]
        public void ToConcatString_ReturnsWithFixedLayoutLeftAndRightWhiteSpaces()
        {
            var _prd = new DTO.WithWhiteSpaces.ProductLeftAndRightWhiteSpacesDTO();

            _prd.Name = "Name of Product";
            _prd.Price = 13.5M;
            _prd.IdCategory = 51;
            _prd.CategoryDescription = "Category Description";


            var _actual = _prd.ToConcatString();
            var _expectedValue = "                                   Name of Product13,5         51Category Description     ";

            Assert.Equal(_expectedValue, _actual);
        }
        [Fact]
        public void ToLoadThisObject_CreateObjBasedStringFixedLayoutLeftAndRightWhiteSpaces()
        {
            bool _expectedValue = true;
            var _actual = new DTO.WithWhiteSpaces.ProductLeftAndRightWhiteSpacesDTO();
            string _concatedValue = "                                   Name of Product13,5         51Category Description     ";

            _actual.ToLoadThisObject(_concatedValue);

            _expectedValue = (_actual.Name == "Name of Product") &&
                (_actual.Price == 13.5M) &&
                (_actual.IdCategory == 51) &&
                (_actual.CategoryDescription == "Category Description");



            Assert.True(_expectedValue);

        }
        [Fact]
        public void ToConcatString_ReturnsWithFixedLayoutSeparedByTabDelimiter()
        {
            var _prd = new ProductWithDelimiterDTO();

            _prd.Name = "Name of Product";
            _prd.Price = 13.5M;
            _prd.IdCategory = 51;
            _prd.CategoryDescription = "Category Description";


            var _actual = _prd.ToConcatString();
            var _expectedValue = "Name of Product                                   	13,5      	51   	Category Description     	";

            Assert.Equal(_expectedValue, _actual);
        }

        [Fact]
        public void ToLoadThisObject_CreateObjBasedStringFixedLayoutSeparedByTabDelimiter()
        {
            bool _expectedValue = true;
            var _actual = new ProductWithDelimiterDTO();
            string _concatedValue = "Name of Product                                   	13,5      	51   	Category Description     	";

            _actual.ToLoadThisObject(_concatedValue);

            _expectedValue = (_actual.Name == "Name of Product") &&
                (_actual.Price == 13.5M) &&
                (_actual.IdCategory == 51) &&
                (_actual.CategoryDescription == "Category Description");



            Assert.True(_expectedValue);

        }

        [Fact]
        public void ToConcatString_ReturnsWithFixedLayoutLeftAndRightWhiteSpacesWithDelimiter()
        {
            var _prd = new DTO.WithWhiteSpaces.WithDelimiter.ProductLeftAndRightWhiteSpacesDTO();

            _prd.Name = "Name of Product";
            _prd.Price = 13.5M;
            _prd.IdCategory = 51;
            _prd.CategoryDescription = "Category Description";


            var _actual = _prd.ToConcatString();
            var _expectedValue = "                                   Name of Product;13,5      ;   51;Category Description     ;";

            Assert.Equal(_expectedValue, _actual);
        }

        [Fact]
        public void ToLoadThisObject_CreateObjBasedStringFixedLayoutLeftAndRightWhiteSpacesWithDelimiter()
        {
            bool _expectedValue = true;
            var _actual = new DTO.WithWhiteSpaces.WithDelimiter.ProductLeftAndRightWhiteSpacesDTO();
            string _concatedValue = "                                   Name of Product;13,5      ;   51;Category Description     ;";

            _actual.ToLoadThisObject(_concatedValue);

            _expectedValue = (_actual.Name == "Name of Product") &&
                (_actual.Price == 13.5M) &&
                (_actual.IdCategory == 51) &&
                (_actual.CategoryDescription == "Category Description");



            Assert.True(_expectedValue);

        }

        [Fact]
        public void ToConcatString_ReturnsWithFixedLayoutLeftWhiteSpacesWithDelimiter()
        {
            var _prd = new DTO.WithWhiteSpaces.WithDelimiter.ProductLeftWhiteSpacesDTO();

            _prd.Name = "Name of Product";
            _prd.Price = 13.5M;
            _prd.IdCategory = 51;
            _prd.CategoryDescription = "Category Description";


            var _actual = _prd.ToConcatString();
            var _expectedValue = "                                   Name of Product;      13,5;   51;     Category Description;";

            Assert.Equal(_expectedValue, _actual);
        }

        [Fact]
        public void ToLoadThisObject_CreateObjBasedStringFixedLayoutLeftWhiteSpacesWithDelimiter()
        {
            bool _expectedValue = true;
            var _actual = new DTO.WithWhiteSpaces.WithDelimiter.ProductLeftWhiteSpacesDTO();
            string _concatedValue = "                                   Name of Product;      13,5;   51;     Category Description;";

            _actual.ToLoadThisObject(_concatedValue);

            _expectedValue = (_actual.Name == "Name of Product") &&
                (_actual.Price == 13.5M) &&
                (_actual.IdCategory == 51) &&
                (_actual.CategoryDescription == "Category Description");



            Assert.True(_expectedValue);

        }

        [Fact]
        public void ToConcatString_ReturnsWithFixedLayoutRightWhiteSpacesWithDelimiter()
        {
            var _prd = new DTO.WithWhiteSpaces.WithDelimiter.ProductRightWhiteSpacesDTO();

            _prd.Name = "Name of Product";
            _prd.Price = 13.5M;
            _prd.IdCategory = 51;
            _prd.CategoryDescription = "Category Description";


            var _actual = _prd.ToConcatString();
            var _expectedValue = "Name of Product                                   -->13,5      -->51   -->Category Description     -->";


            Assert.Equal(_expectedValue, _actual);
        }

        [Fact]
        public void ToLoadThisObject_CreateObjBasedStringFixedLayoutRightWhiteSpacesWithDelimiter()
        {
            bool _expectedValue = true;
            var _actual = new DTO.WithWhiteSpaces.WithDelimiter.ProductRightWhiteSpacesDTO();
            string _concatedValue = "Name of Product                                   -->13,5      -->51   -->Category Description     -->";

            _actual.ToLoadThisObject(_concatedValue);

            _expectedValue = (_actual.Name == "Name of Product") &&
                (_actual.Price == 13.5M) &&
                (_actual.IdCategory == 51) &&
                (_actual.CategoryDescription == "Category Description");



            Assert.True(_expectedValue);

        }

        [Fact]
        public void ToConcatString_ReturnsWithFixedLayoutRightWhiteSpacesWithDelimiterMaxLenght()
        {
            var _prd = new DTO.WithWhiteSpaces.WithDelimiter.ProductRightWhiteSpacesDTO();

            _prd.Name = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUFIM THIS PART SHOULD NOT BE CONSIDERED";
            _prd.Price = 1234567.89M;
            _prd.IdCategory = 1234567890;
            _prd.CategoryDescription = "ABCDEFGHIJKLMNOPRSTUVWFIM THIS PART SHOULD NOT BE CONSIDERED";


            var _actual = _prd.ToConcatString();
            var _expectedValue = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUFIM-->1234567,89-->12345-->ABCDEFGHIJKLMNOPRSTUVWFIM-->";


            Assert.Equal(_expectedValue, _actual);
        }

        [Fact]
        public void ToLoadThisObject_CreateObjBasedStringFixedLayoutRightWhiteSpacesWithDelimiterMaxLenght()
        {
            bool _expectedValue = true;
            var _actual = new DTO.WithWhiteSpaces.WithDelimiter.ProductRightWhiteSpacesDTO();
            string _concatedValue = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUFIM-->1234567,89-->12345-->ABCDEFGHIJKLMNOPRSTUVWFIM-->";

            _actual.ToLoadThisObject(_concatedValue);

            _expectedValue = (_actual.Name == "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUFIM") &&
                (_actual.Price == 1234567.89M) &&
                (_actual.IdCategory == 12345) &&
                (_actual.CategoryDescription == "ABCDEFGHIJKLMNOPRSTUVWFIM");



            Assert.True(_expectedValue);

        }
        [Fact]
        public void ToConcatString_ReturnsWithFixedLayoutWithObjectProperty()
        {
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
            string _expectedValue = "123     " + _expectedDate + "Product number 1                                  1,12      51   Category Description 1   Any note                                                                   ";
            Assert.Equal(_expectedValue, _actual);
        }

        [Fact]
        public void ToLoadThisObject_CreateObjBasedStringFixedLayoutWithObjectProperty()
        {
            bool _expectedValue = true;
            var _actual = new DTO.OrderDTO();
            string _concatedValue = "123     31/08/2019Product number 1                                  1,12      51   Category Description 1   Any note                                                                   ";

            _actual.ToLoadThisObject(_concatedValue);

            _expectedValue = (_actual.OrderId == 123) &&
                        (_actual.CreateDate == Convert.ToDateTime("31/08/2019")) &&
                        (_actual.Notes == "Any note") &&
                        (_actual.Product.Name == "Product number 1") &&
                        (_actual.Product.Price == 1.12M) &&
                        (_actual.Product.IdCategory == 51) &&
                        (_actual.Product.CategoryDescription == "Category Description 1");


            Assert.True(_expectedValue);

        }
        [Fact]
        public void ToConcatString_ReturnsWithFixedLayoutWithIEnumerableObjectProperty()
        {
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
            var _expectedValue = "Report's Name  Alexandre Cardoso        2887    25/05/2019Rice 1kg                                          9,85      298  Foods                    Order to delivery                                                          29877   24/05/2019Protex Soap                                       3,45      296  Body Wash                Some Note                                                                  2887    23/05/2019Beer                                              9,85      298  Drinks                   Order to delivery                                                          8728    21/09/2019";

            Assert.Equal(_expectedValue, _actual);

        }

        [Fact]
        public void ToLoadThisObject_StringFixedLayoutWithIEnumerableObjectProperty()
        {
            //-----------------------------------------------------
            //          Define expected values
            //-----------------------------------------------------
            var _expectedDTO = new ReportOrdersDTO();
            var _ordersList = new List<OrderDTO>();
            var _product_1_DTO = new ProductDTO();
            var _order_1_DTO = new OrderDTO();
            var _product_2_DTO = new ProductDTO();
            var _order_2_DTO = new OrderDTO();
            var _product_3_DTO = new ProductDTO();
            var _order_3_DTO = new OrderDTO();

            _expectedDTO.Name = "Report's Name";
            _expectedDTO.Sponsor = "Alexandre Cardoso";

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

            _expectedDTO.Code = 8728;
            _expectedDTO.CreateDate = Convert.ToDateTime("21/09/2019");

            _expectedDTO.OrderList = _ordersList;

            //-------------------------------------
            //       Declaring Object Type
            //-------------------------------------
            var _actual = new DTO.ReportOrdersDTO();

            //--------------------------------------------------------------------
            //Declare All Qty Array Ocurrences you are expecting
            //In this Unit Test, will be sent a concatenated string with 3 orders
            //--------------------------------------------------------------------
            List<OrderDTO> _orderList = new List<OrderDTO>();
            _orderList.Add(new OrderDTO());
            _orderList.Add(new OrderDTO());
            _orderList.Add(new OrderDTO());
            _actual.OrderList = _orderList;

            //-------------------------------------
            //   Defining string to Test
            //-------------------------------------
            string _concatedValue = "Report's Name  Alexandre Cardoso        2887    25/05/2019Rice 1kg                                          9,85      298  Foods                    Order to delivery                                                          29877   24/05/2019Protex Soap                                       3,45      296  Body Wash                Some Note                                                                  2887    23/05/2019Beer                                              9,85      298  Drinks                   Order to delivery                                                          8728    21/09/2019";

            //-------------------------------------
            //       Run test method
            //-------------------------------------
            _actual.ToLoadThisObject(_concatedValue);



            bool _expectedValue = (_actual.Code == _expectedDTO.Code) &&
            (_actual.CreateDate == _expectedDTO.CreateDate) &&
            (_actual.Name == _expectedDTO.Name) &&
            (_actual.Sponsor == _expectedDTO.Sponsor);

            Assert.True(_expectedValue);

            for (int i = 0; i <= 2; i++)
            {
                _expectedValue =
                    (_actual.OrderList.ToList()[i].CreateDate == _expectedDTO.OrderList.ToList()[i].CreateDate) &&
                    (_actual.OrderList.ToList()[i].Notes == _expectedDTO.OrderList.ToList()[i].Notes) &&
                    (_actual.OrderList.ToList()[i].OrderId == _expectedDTO.OrderList.ToList()[i].OrderId) &&
                    (_actual.OrderList.ToList()[i].Product.CategoryDescription == _expectedDTO.OrderList.ToList()[i].Product.CategoryDescription) &&
                    (_actual.OrderList.ToList()[i].Product.IdCategory == _expectedDTO.OrderList.ToList()[i].Product.IdCategory) &&
                    (_actual.OrderList.ToList()[i].Product.Name == _expectedDTO.OrderList.ToList()[i].Product.Name) &&
                    (_actual.OrderList.ToList()[i].Product.Price == _expectedDTO.OrderList.ToList()[i].Product.Price);


                Assert.True(_expectedValue);
            }

        }

        [Fact]
        public void ToConcatString_ReturnsWithFixedLayoutWithSomeFormatedValues()
        {
            var _2019_saleEntity = Domain.Entity.SaleEntity.Create("Black Friday 2019", new DateTime(2019, 11, 29), 804259.99M);
            var _2020_saleEntity = Domain.Entity.SaleEntity.Create("Black Friday 2020", new DateTime(2019, 11, 27), 110001.10M);
            var _2021_saleEntity = Domain.Entity.SaleEntity.Create("Black Friday 2021", new DateTime(2019, 11, 26), 200002.00M);

            //mapping entity to dto
            var _saleDTO = new SaleDTO();


            _saleDTO.SaleName = _2019_saleEntity.SaleName;
            _saleDTO.EstimatedTotalSale = (_2019_saleEntity.EstimatedTotalSale * 100).ToString("000000000000000");
            _saleDTO.StartDateYear = _2019_saleEntity.StartDate.ToString("yyyy");
            _saleDTO.StartDateMonth = _2019_saleEntity.StartDate.ToString("MM");
            _saleDTO.StartDateDay = _2019_saleEntity.StartDate.ToString("dd");

            var _concatenatedString2019 = _saleDTO.ToConcatString();

            _saleDTO.SaleName = _2020_saleEntity.SaleName;
            _saleDTO.EstimatedTotalSale = (_2020_saleEntity.EstimatedTotalSale * 100).ToString("000000000000000");
            _saleDTO.StartDateYear = _2020_saleEntity.StartDate.ToString("yyyy");
            _saleDTO.StartDateMonth = _2020_saleEntity.StartDate.ToString("MM");
            _saleDTO.StartDateDay = _2020_saleEntity.StartDate.ToString("dd");

            var _concatenatedString2020 = _saleDTO.ToConcatString();

            _saleDTO.SaleName = _2021_saleEntity.SaleName;
            _saleDTO.EstimatedTotalSale = (_2021_saleEntity.EstimatedTotalSale * 100).ToString("000000000000000");
            _saleDTO.StartDateYear = _2021_saleEntity.StartDate.ToString("yyyy");
            _saleDTO.StartDateMonth = _2021_saleEntity.StartDate.ToString("MM");
            _saleDTO.StartDateDay = _2021_saleEntity.StartDate.ToString("dd");

            var _concatenatedString2021 = _saleDTO.ToConcatString();


            Assert.True("20191129000000080425999Black Friday 2019   " == _concatenatedString2019);
            Assert.True("20191127000000011000110Black Friday 2020   " == _concatenatedString2020);
            Assert.True("20191126000000020000200Black Friday 2021   " == _concatenatedString2021);


        }


        [Fact]
        public void ToLoadThisObject_StringFixedLayoutWithSomeFormatedValues()
        {
            string _2019_concatenatedstring = "20191129000000080425999Black Friday 2019   ";
            string _2020_concatenatedstring = "20191127000000011000110Black Friday 2020   ";
            string _2021_concatenatedstring = "20191126000000020000200Black Friday 2021   ";

            var _saleDTO2019 = new SaleDTO();
            var _saleDTO2020 = new SaleDTO();
            var _saleDTO2021 = new SaleDTO();

            _saleDTO2019.ToLoadThisObject(_2019_concatenatedstring);
            _saleDTO2020.ToLoadThisObject(_2020_concatenatedstring);
            _saleDTO2021.ToLoadThisObject(_2021_concatenatedstring);


            

            var _2019_saleEntity = Domain.Entity.SaleEntity.Create(_saleDTO2019.SaleName, 
                new DateTime(Convert.ToInt16(_saleDTO2019.StartDateYear), 
                               Convert.ToInt16(_saleDTO2019.StartDateMonth), 
                               Convert.ToInt16(_saleDTO2019.StartDateDay)), 
                   Convert.ToDecimal(_saleDTO2019.EstimatedTotalSale) / 100);

            var _2020_saleEntity = Domain.Entity.SaleEntity.Create(_saleDTO2020.SaleName,
                new DateTime(Convert.ToInt16(_saleDTO2020.StartDateYear),
                                Convert.ToInt16(_saleDTO2020.StartDateMonth),
                                Convert.ToInt16(_saleDTO2020.StartDateDay)),
                    Convert.ToDecimal(_saleDTO2020.EstimatedTotalSale) / 100);

            var _2021_saleEntity = Domain.Entity.SaleEntity.Create(_saleDTO2021.SaleName,
                new DateTime(Convert.ToInt16(_saleDTO2021.StartDateYear),
                               Convert.ToInt16(_saleDTO2021.StartDateMonth),
                               Convert.ToInt16(_saleDTO2021.StartDateDay)),
                   Convert.ToDecimal(_saleDTO2021.EstimatedTotalSale) / 100);


            Assert.True((_2019_saleEntity.SaleName.Trim() == "Black Friday 2019") == true &&
                (_2019_saleEntity.StartDate == new DateTime(2019, 11, 29, 0, 0, 0) == true &&
                (_2019_saleEntity.EstimatedTotalSale == 804259.99M) == true));

            Assert.True((_2020_saleEntity.SaleName.Trim() == "Black Friday 2020") == true &&
                (_2020_saleEntity.StartDate == new DateTime(2019, 11, 27, 0, 0, 0) == true &&
                (_2020_saleEntity.EstimatedTotalSale == 110001.10M) == true));

            Assert.True((_2021_saleEntity.SaleName.Trim() == "Black Friday 2021") == true &&
                (_2021_saleEntity.StartDate == new DateTime(2019, 11, 26, 0, 0, 0) == true &&
                (_2021_saleEntity.EstimatedTotalSale == 200002.00M) == true));

        }


    }
}
