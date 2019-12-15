using BookstoreAppLib.DomainModelLayer.Store;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookstoreAppLib.Tests.Tests
{    
    [TestFixture(null, 0d)]
    [TestFixture("", 0d)]        
    [TestFixture("cat1", 1d)]
    [TestFixture("cat1", -0.000000000000001)]
    class CategoryDomainModelTests
    {
        private string _name;
        private decimal _discount;

        public CategoryDomainModelTests(string name, double discount)
        {
            _name = name;
            _discount = (decimal)discount;
        }

        [Test]
        public void When_CategoryNameOrDiscountIsInvalid_Expect_Exception()
        {            
            Assert.Throws<FluentValidation.ValidationException>(() => new Category(_name, _discount));
        }
    }
}
