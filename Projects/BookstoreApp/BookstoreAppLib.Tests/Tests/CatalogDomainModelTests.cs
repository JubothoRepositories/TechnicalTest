using BookstoreAppLib.DomainModelLayer.Store;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookstoreAppLib.Tests.Tests
{
    //name, price, quantity
    [TestFixture]    
    class CatalogDomainModelTests
    {                
        [TestCase(null, 1d, 1)]
        [TestCase("", 1d, 1)]       
        [TestCase("book1", 0, 1)]
        [TestCase("book1", 1, -1)]
        public void When_CatalogNameOrPriceOrQuantityIsInvalid_Expect_Exception(string _name, decimal _price, int _quantity)
        {            
            Assert.Throws<FluentValidation.ValidationException>(() => new Catalog(_name, new Category("noDiscountCategory", 0), _price, _quantity));
        }

        [Test]
        public void When_CatalogCategoryIsNull_Expect_Exception()
        {
            Assert.Throws<FluentValidation.ValidationException>(() => new Catalog("Book1", null, 1, 1));
        }
    }
}
