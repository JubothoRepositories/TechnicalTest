using BookstoreAppLib.ApplicationLayer.Store;
using BookstoreAppLib.DomainModelLayer.Store;
using BookstoreAppLib.Exceptions;
using BookstoreAppLib.Exceptions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreAppLib.InfrastructureLayer.BasketCalculators
{
    /// <summary>
    /// Calculates the price of a basket
    /// </summary>
    public class BasketCalcTechRule : IBasketCalculator
    {
        /// <summary>
        /// Calculates the price of a basket
        /// </summary>
        /// <param name="catalogs">Desired books</param>
        /// <returns></returns>
        public decimal CalculateCatalogPriceAsync(IReadOnlyCollection<Catalog> catalogs)
        {
            if (catalogs.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(catalogs), catalogs.Count, "Invalid catalogs count");

            //The .ToList() is required in order to access the .ForEach

            decimal totalSum = 0m;
            List<INameQuantity> missingBooks = new List<INameQuantity>();

            //group by category
            catalogs.GroupBy(g => g.Category)
                .ToList()
                .ForEach(category =>
                {
                    bool moreThanOneBookInCurrentCategory = category.Count() > 1;

                    //group by books in a category
                    category.GroupBy(g => new { g.Name, g.Price, g.Quantity })
                        .ToList()
                        .ForEach(sameBooksInCategory =>
                        {
                            if (sameBooksInCategory.Count() > sameBooksInCategory.Key.Quantity)
                            {
                                missingBooks.Add(new NameQuantity(sameBooksInCategory.Key.Name, sameBooksInCategory.Key.Quantity));
                            }
                            else
                            {
                                if (missingBooks.Count() == 0)
                                {
                                    //no need to use it that way, but just showing the new switch expression in c# 8
                                    totalSum += moreThanOneBookInCurrentCategory switch
                                    {
                                        true => sameBooksInCategory.Key.Price * (1 - category.Key.Discount) + (sameBooksInCategory.Count() - 1) * sameBooksInCategory.Key.Price,
                                        false => sameBooksInCategory.Count() * sameBooksInCategory.Key.Price
                                    };
                                }
                            }
                        });
                });

            if (missingBooks.Count > 0)
            {
                throw new NotEnoughInventoryException(missingBooks);
            }

            return totalSum;
        }
    }
}
