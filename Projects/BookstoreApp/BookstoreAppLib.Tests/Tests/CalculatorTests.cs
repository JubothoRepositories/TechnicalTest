using BookstoreAppLib.ApplicationLayer.Store;
using BookstoreAppLib.DomainModelLayer.Store;
using BookstoreAppLib.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookstoreAppLib.Tests.Tests
{
    [TestFixture]
    class CalculatorTests
    {
        private Catalog Book1 = new Catalog("book1", new Category("noDiscountCategory", 0), 100, 10);
        private Catalog Book2 = new Catalog("book2", new Category("10percentageDiscountCategory", 0.10m), 100, 2);
        private Catalog Book3 = new Catalog("book3", new Category("20percentageDiscountCategory", 0.20m), 100, 3);

        [Test]
        public void When_Buy1Book_Expect_ItsPrice()
        {
            IBasketCalculator calc = new InfrastructureLayer.BasketCalculators.BasketCalcTechRule();

            decimal price = calc.CalculateCatalogPrice(new List<Catalog>()
            {
                Book1
            });

            Assert.AreEqual(price, Book1.Price);
        }

        [Test]
        public void When_Buy0Books_Expect_Exception()
        {
            IBasketCalculator calc = new InfrastructureLayer.BasketCalculators.BasketCalcTechRule();

            Assert.Throws<ArgumentOutOfRangeException>(() => calc.CalculateCatalogPrice(new List<Catalog>()));
        }

        [Test]
        public void When_BuyNullBooks_Expect_Exception()
        {
            IBasketCalculator calc = new InfrastructureLayer.BasketCalculators.BasketCalcTechRule();

            Assert.Throws<ArgumentOutOfRangeException>(() => calc.CalculateCatalogPrice(null));
        }

        [Test]
        public void When_BuyMoreThanAvaliableBooks_Expect_Exception()
        {
            IBasketCalculator calc = new InfrastructureLayer.BasketCalculators.BasketCalcTechRule();

            NotEnoughInventoryException exception = Assert.Throws<NotEnoughInventoryException>(() => calc.CalculateCatalogPrice(new List<Catalog>() { Book2, Book2, Book2 }));
            Assert.AreEqual(Book2.Name, exception.Missing.First().Name);
            Assert.AreEqual(Book2.Quantity, exception.Missing.First().Quantity);
        }

        [Test]
        public void When_BuyBooksInSameGroup_Expect_DiscountForTheFirstOnly()
        {
            IBasketCalculator calc = new InfrastructureLayer.BasketCalculators.BasketCalcTechRule();

            decimal price = calc.CalculateCatalogPrice(new List<Catalog>()
            {
                Book1, Book1, Book1,
                Book2, Book2
            });

            Assert.AreEqual(Book1.Price * 3 + Book2.Price * (1 - Book2.Category.Discount) + Book2.Price, price);
        }

        [Test]
        public void When_BuyBooksInSameGroup_Expect_DiscountForTheFirstOnly3Groups()
        {
            IBasketCalculator calc = new InfrastructureLayer.BasketCalculators.BasketCalcTechRule();

            decimal price = calc.CalculateCatalogPrice(new List<Catalog>()
            {
                Book1, Book1, Book1,
                Book2, Book2,
                Book3, Book3, Book3
            });

            Assert.AreEqual(
                Book1.Price * (1 - Book1.Category.Discount) + 2 * Book1.Price +
                Book2.Price * (1 - Book2.Category.Discount) + 1 * Book2.Price +
                Book3.Price * (1 - Book3.Category.Discount) + 2 * Book3.Price,
                price);
        }
    }
}
