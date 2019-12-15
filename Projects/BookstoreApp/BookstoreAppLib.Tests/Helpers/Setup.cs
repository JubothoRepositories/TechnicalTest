using BookstoreAppLib.ApplicationLayer.Store;
using BookstoreAppLib.InfrastructureLayer.BasketCalculators;
using BookstoreAppLib.InfrastructureLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BookstoreAppLib.Tests
{
    internal static class Setup
    {
        internal static JsonInMemoryStoreRepository InitFullStore()
        {
            var calcService = new BasketCalcTechRule();
            var store = new JsonInMemoryStoreRepository(Setup.GetRealJsonSchema, calcService);
            store.Import(Setup.GetRealStoreJson);

            return store;
        }

        internal static string GetRealJsonSchema => File.ReadAllText(@".\BookstoreDb\schema-store.json");
        internal static string GetRealStoreJson => File.ReadAllText(@".\BookstoreDb\content-store.json");

        internal const string BadJson = "{\n\"This\" \ndoes not looks like a valid json";
        internal const string Category_ScienceFiction = @"{'Category': [ { 'Name': 'Science Fiction', 'Discount': 0.05 }],'Catalog': []}";
        internal const string Catalog_Goblet = @"{'Category': [],'Catalog': [{'Name': 'J.K Rowling - Goblet Of fire','Category': 'Fantastique', 'Price': 8,'Quantity': 2}]}";

        internal const string NotExistingBookName = "Jules Verne - Journey to the Center of the Earth";
    }
}
