using BookstoreAppLib.Exceptions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookstoreAppLib.Exceptions
{
    /// <summary>
    /// If a shopping cart is invalid because the catalog does not contain enough books 
    /// </summary>
    public class NotEnoughInventoryException : Exception
    {
        /// <summary>
        /// Containing the list of books not found
        /// </summary>
        public IEnumerable<INameQuantity> Missing { get; }

        public NotEnoughInventoryException(IEnumerable<INameQuantity> missing)
        : base("Not Enough Inventory")
        {
            Missing = missing;
        }
    }

    public class NameQuantity : INameQuantity
    {
        public NameQuantity(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public string Name { get; set; }

        public int Quantity { get; set; }
    }

}