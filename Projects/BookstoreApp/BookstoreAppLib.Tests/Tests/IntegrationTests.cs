using BookstoreAppLib.InfrastructureLayer.BasketCalculators;
using BookstoreAppLib.InfrastructureLayer.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookstoreAppLib.Tests.Tests
{
    [TestFixture]
    class IntegrationTests
    {        
        [Test]
        public void When_GetQuantityOfExistingBook_Expect_ItsQuantity()
        {
            JsonInMemoryStoreRepository store = Setup.InitFullStore();

            int quantity = store.Quantity("Ayn Rand - FountainHead");

            Assert.AreEqual(10, quantity);
        }

        [Test]
        public void When_CalculateBasket_Expect_CorrectResult()
        {
            JsonInMemoryStoreRepository store = Setup.InitFullStore();

            double quantity = store.Buy(
                "J.K Rowling - Goblet Of fire", 
                "Isaac Asimov - Foundation");

            Assert.AreEqual(24, quantity);
        }

        [Test]
        public void When_CalculateBasketMultipleBooks_Expect_OnlyFirstBookHaveDiscount()
        {
            JsonInMemoryStoreRepository store = Setup.InitFullStore();

            double quantity = store.Buy(
                "J.K Rowling - Goblet Of fire",
                "Robin Hobb - Assassin Apprentice",
                "Robin Hobb - Assassin Apprentice");

            Assert.AreEqual(30, quantity);
        }

        [Test]
        public void When_CalculateBasketMultipleBooks_Expect_OnlyFirstBookHaveDiscount_Example2()
        {
            JsonInMemoryStoreRepository store = Setup.InitFullStore();

            double quantity = store.Buy(
                "Ayn Rand - FountainHead",
                "Isaac Asimov - Foundation",
                "Isaac Asimov - Robot series",
                "J.K Rowling - Goblet Of fire",
                "J.K Rowling - Goblet Of fire",
                "Robin Hobb - Assassin Apprentice",
                "Robin Hobb - Assassin Apprentice");

            Assert.AreEqual(69.95, quantity);
        }

    }
}
