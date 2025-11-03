using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators
{
    public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
    {
        private readonly int[] _allowedPageSizes = new[] { 5, 10, 15 };
        private readonly string[] _allowedSortByColumns = new[] { nameof(Restaurant.Name), nameof(Restaurant.Description), nameof(Restaurant.Category) };
        public RestaurantQueryValidator()
        {
            RuleFor(r => r.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(r => r.PageSize)
                .Custom((value, context) =>
                {
                    if (!_allowedPageSizes.Contains(value))
                    {
                        context.AddFailure($"Page size must be one of the following values: {string.Join(", ", _allowedPageSizes)}");
                    }
                });

            RuleFor(r => r.SearchPhrase)
                .MaximumLength(100)
                .WithMessage("Search phrase must not exceed 100 characters.");

            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || _allowedSortByColumns.Contains(value))
                .WithMessage($"Sort by must be one of the following values: {string.Join(", ", _allowedSortByColumns)}");
        }
    }
}