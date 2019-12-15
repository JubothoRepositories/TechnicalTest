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
    public static class Setup
    {
        //private static (IStore, IBasketCalculator) ConfigureServices()
        //{
        //    string jsonSchema = GetRealJsonSchema;

        //    //Setup DI
        //    var services = new ServiceCollection();

        //    services
        //        .AddScoped<IBasketCalculator, BasketCalcTechRule>()
        //        .AddScoped<IStore, JsonInMemoryStoreRepository>(d => new JsonInMemoryStoreRepository(jsonSchema, services.BuildServiceProvider().GetService<IBasketCalculator>()));
        //    ServiceProvider serviceProvider = services.BuildServiceProvider();

        //    return (
        //            serviceProvider.GetService<IStore>(),
        //            serviceProvider.GetService<IBasketCalculator>());
        //}

        //private static void LoadFullCatalog(IStore store)
        //{
        //    store.Import(GetRealStoreJson);
        //}

        //public static (IStore store, IBasketCalculator calc) GetFullInventory()
        //{
        //    (IStore store, IBasketCalculator calc) = ConfigureServices();
        //    LoadFullCatalog(store);

        //    return (store, calc);
        //}

        public static string GetRealJsonSchema => File.ReadAllText(@".\BookstoreDb\schema-store.json");
        public static string GetRealStoreJson => File.ReadAllText(@".\BookstoreDb\content-store.json");

        public const string BadJson = "{\n\"This\" \ndoes not looks like a valid json";
        public const string Category_ScienceFiction = @"{'Category': [ { 'Name': 'Science Fiction', 'Discount': 0.05 }],'Catalog': []}";
        public const string Catalog_Goblet = @"{'Category': [],'Catalog': [{'Name': 'J.K Rowling - Goblet Of fire','Category': 'Fantastique', 'Price': 8,'Quantity': 2}]}";

        public const string NotExistingBookName = "Jules Verne - Journey to the Center of the Earth";
    }
}
