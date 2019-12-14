using BookstoreAppLib.DomainModelLayer.Store;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreAppLib.ApplicationLayer.Store
{
    /// <summary>
    /// Calculates the price of the basket
    /// </summary>
    public interface IBasketCalculator
    {
        decimal CalculateCatalogPriceAsync(IReadOnlyCollection<Catalog> catalogs);
    }
}
