using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookstoreAppLib.DomainModelLayer.Store
{
    /// <summary>
    /// One category with its discount
    /// </summary>
    public class Category
    {
        /// <summary>
        /// The unique name of the category, it is a functional key
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The discount applies when buying multiple book of this category
        /// </summary>
        public decimal Discount { get; }

        public Category(string name, decimal discount)
        {            
            Name = name;
            Discount = discount;

            var validator = new CategoryValidator();
            validator.Validate(this);
        }

        public class CategoryValidator : AbstractValidator<Category>
        {
            public CategoryValidator()
            {
                RuleFor(category => category.Name).NotNull().NotEmpty();
                RuleFor(category => category.Discount).LessThan(1m).GreaterThanOrEqualTo(0m);
            }
        }

    }
}
