using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreAppLib.ApplicationLayer.Store
{
    /// <summary>
    /// Definition of interfaces and classes
    /// </summary>
    public interface IStore
    {
        /// <summary>
        /// import the stock data from the library from a JSON file whose format is specified in Appendix 1.
        /// </summary>
        /// <param name="catalogAsJson">The json representing the catalog</param>
        void Import(string catalogAsJson);

        /// <summary>
        /// Returns the available stock by the book title, 
        /// </summary>
        /// <param name="name">The title of a book</param>
        /// <returns>returns the number of copies available</returns>
        int Quantity(string name);

        /// <summary>
        /// Calculates the price of the basket
        /// </summary>
        /// <param name="basketByNames">List of the book titles</param>
        /// <returns>Calculates the price of the basket</returns>
        double Buy(params string[] basketByNames);
    }
}