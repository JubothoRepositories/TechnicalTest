using BookstoreAppLib.ApplicationLayer.Store;
using BookstoreAppLib.DomainModelLayer.Store;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookstoreAppLib.InfrastructureLayer.Repositories
{
    public sealed class JsonInMemoryStoreRepository : IStore
    {
        private readonly string _jsonSchema;
        private readonly IBasketCalculator _basketCalculator;
        private JSchema schema;

        //our in memory repository
        private IList<Category> _categories = new List<Category>();
        private IList<Catalog> _catalogs = new List<Catalog>();

        //our primary key checker, using hashset is a lot faster than just checking is that name already in the list
        private HashSet<string> _categoryNames = new HashSet<string>();
        private HashSet<string> _catalogNames = new HashSet<string>();

        /// <summary>
        /// Our store catalogs
        /// </summary>
        public IReadOnlyList<Catalog> Catalogs => (IReadOnlyList<Catalog>)_catalogs;

        /// <summary>
        /// Our store Categories
        /// </summary>
        public IReadOnlyList<Category> Categories => (IReadOnlyList<Category>)_categories;

        public JsonInMemoryStoreRepository(string jsonSchema, IBasketCalculator basketCalculator)
        {
            _jsonSchema = jsonSchema;
            _basketCalculator = basketCalculator;
        }

        public double Buy(params string[] basketByNames)
        {
            EnsureStoreIsValid();

            ICollection<Catalog> basket = new List<Catalog>();

            foreach(var bookName in basketByNames)
            {
                basket.Add(GetCatalogByName(bookName));
            }

            return (double)_basketCalculator.CalculateCatalogPrice((IReadOnlyCollection<Catalog>)basket);
        }


        /// <summary>
        /// import the stock data from the library from a JSON file whose format is specified in Appendix 1.
        /// </summary>
        /// <param name="catalogAsJson">The json representing the catalog</param>
        public void Import(string catalogAsJson)
        {
            if (schema == null)
            {
                schema = JSchema.Parse(_jsonSchema);
            }

            JObject bookStoreJsonContent = JObject.Parse(catalogAsJson);
            
            if (!bookStoreJsonContent.IsValid(schema, out IList<string> errors))
            {
                throw new ArgumentException($"Bad json format, errors: {string.Join(',', errors)}");
            }

            //please note that the discount is guaranteed to be decimal because of the json schema validator
            //we don't need and should not validate again
            //we are using very strict rules when to continue, we don't allow invalid state to exists
            //this includes the domain objects constructors as well            
            bookStoreJsonContent["Category"]
                .Children()
                .ToList()
                .ForEach(f =>
                {
                    //The unique name of the category, it is a functionnal key
                    //We could check on each addition is the category/catalog already inserted, but would be slow                    
                    string name = f["Name"].Value<string>();
                    if (_categoryNames.Contains(name))
                    {
                        throw new ArgumentException($"A category with name '{name}' already exists!");
                    }

                    _categoryNames.Add(name);
                    _categories.Add(new Category(
                        name,
                        f["Discount"].Value<decimal>()));
                });

            bookStoreJsonContent["Catalog"]
                .Children()
                .ToList()
                .ForEach(f =>
                {
                    //The unique Name of the book, it is a functionnal key
                    string name = f["Name"].Value<string>();
                    if (_catalogNames.Contains(name))
                    {
                        throw new ArgumentException($"A catalog with name '{name}' already exists!");
                    }

                    _catalogNames.Add(name);
                    _catalogs.Add(new Catalog(
                        name,
                        _categories.Single(s => s.Name.Equals(f["Category"].Value<string>())),
                        f["Price"].Value<decimal>(),
                        f["Quantity"].Value<int>()
                        ));
                });
        }

        /// <summary>
        /// Returns the available stock by the book title, 
        /// </summary>
        /// <param name="name">The title of a book</param>
        /// <returns>returns the number of copies available</returns>
        public int Quantity(string name)
        {
            EnsureStoreIsValid();
            Catalog catalog = GetCatalogByName(name);

            return catalog.Quantity;
        }

        private Catalog GetCatalogByName(string name)
        {
            Catalog catalog = _catalogs.SingleOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (catalog is null)
            {
                throw new ArgumentException($"Sorry, a book with name '{name}' does not exist in our store!");
            }

            return catalog;
        }

        private void EnsureStoreIsValid()
        {
            if (!_catalogs.Any() || !_categories.Any())
            {
                throw new Exception("The store is not initialized");
            }
        }
    }
}