using BookstoreAppLib.ApplicationLayer.Store;
using BookstoreAppLib.InfrastructureLayer.BasketCalculators;
using BookstoreAppLib.InfrastructureLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace BookstoreAppConsoleTest
{
    class Program
    {
        private const string SchemaLocation = @".\BookstoreDb\schema-store.json";
        private const string JsonLocation = @".\BookstoreDb\content-store.json";

        static void Main(string[] args)
        {
            string jsonSchema = File.ReadAllText(SchemaLocation);
            ServiceCollection services = ConfigureServices(jsonSchema);

            var serviceProvider = services.BuildServiceProvider();
            var store = serviceProvider.GetService<IStore>();

            store.Import(File.ReadAllText(JsonLocation));

            store.Quantity("Isaac Asimov - Foundation");

            double cost = store.Buy("Ayn Rand - FountainHead",
                "Isaac Asimov - Foundation",
                "Isaac Asimov - Robot series",
                "J.K Rowling - Goblet Of fire",
                "J.K Rowling - Goblet Of fire",
                "Robin Hobb - Assassin Apprentice",
                "Robin Hobb - Assassin Apprentice");
        }

        private static ServiceCollection ConfigureServices(string jsonSchema)
        {
            var services = new ServiceCollection();

            return (ServiceCollection)services
                .AddScoped<IBasketCalculator, BasketCalcTechRule>()
                .AddScoped<IStore, JsonInMemoryStoreRepository>(d => new JsonInMemoryStoreRepository(jsonSchema, services.BuildServiceProvider().GetService<IBasketCalculator>()));
        }
    }
}
