using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookstoreAppLib.DomainModelLayer.Store
{
    public class Catalog
    {

        /// <summary>
        /// The unique Name of the book, it is a functional key
        /// </summary>
        /// <example>J.K Rowling - Goblet Of fire</example>
        public string Name { get; }

        /// <summary>
        /// The name of one the category existing in the Category root properties.
        /// </summary>
        public Category Category { get; }

        /// <summary>
        /// The price of an copy of the book
        /// </summary>
        public decimal Price { get; }

        /// <summary>
        /// The Quantity of copy of the book in the catalog
        /// </summary>
        public int Quantity { get; }

        public Catalog(string name, Category category, decimal price, int quantity)
        {
            Name = name;
            Category = category;
            Price = price;
            Quantity = quantity;

            var validator = new CatalogValidator();
            validator.ValidateAndThrow(this);
        }

        public class CatalogValidator : AbstractValidator<Catalog>
        {
            public CatalogValidator()
            {
                RuleFor(catalog => catalog.Category).NotNull();
                RuleFor(catalog => catalog.Name).NotNull().NotEmpty();
                RuleFor(catalog => catalog.Price).GreaterThan(0m);
                RuleFor(catalog => catalog.Quantity).GreaterThanOrEqualTo(0);
            }
        }

    }
}
