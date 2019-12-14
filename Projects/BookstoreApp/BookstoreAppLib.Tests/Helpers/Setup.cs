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
        private static (IStore, IBasketCalculator) ConfigureServices()
        {
            string jsonSchema = File.ReadAllText(@".\BookstoreDb\schema-store.json");

            //Setup DI
            var services = new ServiceCollection();

            services
                .AddScoped<IBasketCalculator, BasketCalcTechRule>()
                .AddScoped<IStore, JsonInMemoryStoreRepository>(d => new JsonInMemoryStoreRepository(jsonSchema, services.BuildServiceProvider().GetService<IBasketCalculator>()));
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            return (
                    serviceProvider.GetService<IStore>(),
                    serviceProvider.GetService<IBasketCalculator>());
        }

        private static void LoadFullCatalog(IStore store)
        {
            store.Import(File.ReadAllText(@".\BookstoreDb\content-store.json"));
        }

        public static (IStore store, IBasketCalculator calc) GetFullInventory()
        {
            (IStore store, IBasketCalculator calc) = ConfigureServices();
            LoadFullCatalog(store);

            return (store, calc);
        }
    }
}
