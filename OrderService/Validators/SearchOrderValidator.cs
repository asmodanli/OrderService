using FluentValidation;
using OrderService.Models;

namespace OrderService.Validators
{
    public class SearchOrderValidator : AbstractValidator<OrderFilter>
    {
        public SearchOrderValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("You should give body");

            When(instance => instance != null, () =>
            {
                RuleFor(x => x.PageSize).NotNull().WithMessage("PageSize is required!")
                    .GreaterThan(0).WithMessage("PageSize is required!");
                RuleFor(x => x.PageNumber).NotNull().WithMessage("PageNumber is required!")
                    .GreaterThan(0).WithMessage("PageNumber is required!");
                RuleFor(x => x.SearchText).NotNull().WithMessage("SearchText is required!");
                RuleForEach(x => x.Statuses).NotNull().WithMessage("Statuses are required!")
                    .IsInEnum().WithMessage("Statu values are not valid!");
                RuleFor(x => x.SortBy).NotNull().WithMessage("SortBy is required!");
                RuleFor(x => x.EndDate)
                    .NotNull().WithMessage("EndDate is required!")
                    .GreaterThan(x => x.StartDate).WithMessage("EndDate must be greater than StartDate");
                RuleFor(x => x.StartDate)
                    .NotNull().WithMessage("StartDate is required!")
                    .LessThan(x => x.EndDate).WithMessage("StartDate must be less than StartDate");
            });
        }

    }
}
