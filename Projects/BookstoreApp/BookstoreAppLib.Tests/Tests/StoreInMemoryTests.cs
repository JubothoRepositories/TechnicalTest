using BookstoreAppLib.ApplicationLayer.Store;
using BookstoreAppLib.DomainModelLayer.Store;
using BookstoreAppLib.InfrastructureLayer.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookstoreAppLib.Tests.Tests
{
    [TestFixture]
    class StoreInMemoryTests
    {        
        [Test]
        public void When_TryToImportNullJsonSchema_Expect_Exception()
        {            
            var store = new JsonInMemoryStoreRepository(null, null);

            var ex = Assert.Throws<ArgumentNullException>(() => store.Import(null));

            Assert.AreEqual("json", ex.ParamName);
        }

        [Test]
        public void When_TryToImportEmptyJsonSchema_Expect_Exception()
        {            
            var store = new JsonInMemoryStoreRepository(string.Empty, null);

            var ex = Assert.Throws<Newtonsoft.Json.Schema.JSchemaReaderException>(() => store.Import(null));

            Assert.AreEqual(0, ex.LineNumber);
            Assert.AreEqual(0, ex.LinePosition);
        }

        [Test]
        public void When_TryToImportBadJsonSchema_Expect_Exception()
        {
            var store = new JsonInMemoryStoreRepository(Setup.BadJson, null);

            var ex = Assert.Throws<Newtonsoft.Json.JsonReaderException>(() => store.Import(null));

            Assert.AreEqual(3, ex.LineNumber);
            Assert.AreEqual(0, ex.LinePosition);
        }

        [Test]
        public void When_TryToImportBadJson_Expect_Exception()
        {
            var store = new JsonInMemoryStoreRepository("{}", null);

            var ex = Assert.Throws<Newtonsoft.Json.JsonReaderException>(() => store.Import(Setup.BadJson));

            Assert.AreEqual(3, ex.LineNumber);
            Assert.AreEqual(0, ex.LinePosition);
        }

        [Test]
        public void When_TryToImportEmptyJson_Expect_Exception()
        {
            var store = new JsonInMemoryStoreRepository("{}", null);

            var ex = Assert.Throws<Newtonsoft.Json.JsonReaderException>(() => store.Import(string.Empty));

            Assert.AreEqual(0, ex.LineNumber);
            Assert.AreEqual(0, ex.LinePosition);
        }

        [Test]
        public void When_TryToImportJsonWhichNotFitSchema_Expect_Exception()
        {
            var store = new JsonInMemoryStoreRepository("{\"required\": [\"a\"]}", null);

            Assert.Throws<ArgumentException>(() => store.Import("{\"b\":null}"));
        }

        [Test]
        public void When_ImportGoodJsonAndGoodSchema_Expect_Success()
        {
            var store = new JsonInMemoryStoreRepository(Setup.GetRealJsonSchema, null);
            
            store.Import(Setup.GetRealStoreJson);

            Assert.AreEqual(3, store.Categories.Count);
            Assert.AreEqual(5, store.Catalogs.Count);
        }

        [Test]
        public void When_TryToImportSameCategory_Expect_Exception()
        {
            var store = new JsonInMemoryStoreRepository(Setup.GetRealJsonSchema, null);
            store.Import(Setup.GetRealStoreJson);

            Assert.Throws<ArgumentException>(() => store.Import(Setup.Category_ScienceFiction));
        }

        [Test]
        public void When_TryToImportSameCatalog_Expect_Exception()
        {
            var store = new JsonInMemoryStoreRepository(Setup.GetRealJsonSchema, null);
            store.Import(Setup.GetRealStoreJson);

            Assert.Throws<ArgumentException>(() => store.Import(Setup.Catalog_Goblet));
        }

        [Test]
        public void When_TryToBuyButStoreIsEmpty_Expect_Exception()
        {
            var store = new JsonInMemoryStoreRepository("{}", null);
            Assert.Throws<Exception>(() => store.Buy(null));
        }

        [Test]
        public void When_TryToBuyNotExistingBook_Expect_Exception()
        {
            var store = new JsonInMemoryStoreRepository(Setup.GetRealJsonSchema, null);
            store.Import(Setup.GetRealStoreJson);

            Assert.Throws<ArgumentException>(() => store.Buy("Jules Verne - Journey to the Center of the Earth"));
        }

        [Test]
        public void When_TryToBuyExistingBooks_Expect_Success()
        {
            decimal expectedPrice = 1.23m;

            var mock = new Mock<IBasketCalculator>();
            mock.Setup(s => s.CalculateCatalogPrice(It.IsAny<IReadOnlyCollection<Catalog>>())).Returns(expectedPrice);

            var store = new JsonInMemoryStoreRepository(Setup.GetRealJsonSchema, mock.Object);
            store.Import(Setup.GetRealStoreJson);

            double basketPrice = store.Buy("Isaac Asimov - Foundation");

            Assert.AreEqual(expectedPrice, basketPrice);
        }

        [Test]
        public void When_TryToGetQuantityButStoreIsEmpty_Expect_Exception()
        {
            var store = new JsonInMemoryStoreRepository("{}", null);
            Assert.Throws<Exception>(() => store.Quantity(null));
        }

        [Test]
        public void When_TryToGetQuantityOfNotExistingBook_Expect_Exception()
        {
            var store = new JsonInMemoryStoreRepository(Setup.GetRealJsonSchema, null);
            store.Import(Setup.GetRealStoreJson);

            Assert.Throws<ArgumentException>(() => store.Quantity(Setup.NotExistingBookName));
        }

        [Test]
        public void When_TryToGetQuantityOfExistingBooks_Expect_Success()
        {            
            var store = new JsonInMemoryStoreRepository(Setup.GetRealJsonSchema, null);
            store.Import(Setup.GetRealStoreJson);

            int quantity = store.Quantity("Isaac Asimov - Foundation");

            Assert.AreEqual(1, quantity);
        }
    }
}
