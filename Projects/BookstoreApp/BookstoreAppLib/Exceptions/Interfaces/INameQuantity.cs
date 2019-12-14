using System;
using System.Collections.Generic;
using System.Text;

namespace BookstoreAppLib.Exceptions.Interfaces
{
    /// <summary>
    /// containing the list of books not found
    /// </summary>
    public interface INameQuantity
    {
        /// <summary>
        /// The title of a book
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The number of AVALIABLE copies
        /// </summary>
        int Quantity { get; }
    }

}