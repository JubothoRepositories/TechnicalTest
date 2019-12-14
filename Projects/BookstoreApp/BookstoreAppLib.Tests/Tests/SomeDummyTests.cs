using BookstoreAppLib.ApplicationLayer.Store;
using BookstoreAppLib.InfrastructureLayer.BasketCalculators;
using BookstoreAppLib.InfrastructureLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.IO;

namespace BookstoreAppLib.Tests
{
    //[TestFixture("J.K Rowling - Goblet Of fire", 8)]
    //[TestFixture("Ayn Rand - FountainHead", 12)]
    //public class SomeDummyTests
    //{
    //    private string _bookName;
    //    private double _cost;

    //    public SomeDummyTests(string bookName, double cost)
    //    {
    //        _bookName = bookName;
    //        _cost = cost;
    //    }

    //    [Test]
    //    public void When_SingleBook_Expect_ItsPrice()
    //    {
    //        (IStore store, IBasketCalculator calc) = Setup.GetFullInventory();
            
    //        double price = store.Buy(_bookName);

    //        Assert.That(price == _cost);
    //    }

    //    [Test]
    //    public void When_SingleBookWithWrongPrice_Expect_ItsPriceNotMatching()
    //    {
    //        (IStore store, IBasketCalculator calc) = Setup.GetFullInventory();

    //        double price = store.Buy(_bookName);

    //        Assert.That(price != _cost + 1);
    //    }
    //}
}
