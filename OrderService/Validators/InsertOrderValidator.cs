using FluentValidation;
using OrderService.Models;
using System.Security.Cryptography.X509Certificates;

namespace OrderService.Validators
{
    public class InsertOrdersValidator : AbstractValidator<Order>
    {
        public InsertOrdersValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("You should give body");


            {
                RuleFor(x => x.Id).NotNull().WithMessage("Id is required!")
                    .GreaterThan(0).WithMessage("Id must be greater than zero!");
                RuleFor(x => x.BrandId).NotNull().WithMessage("BrandId is required!")
                   .GreaterThan(0).WithMessage("BrandId must be greater than zero!");
                RuleFor(x => x.CustomerName).NotNull().WithMessage("CustomerName is requiered!");
                RuleFor(x => x.Price).NotNull().WithMessage("Price is required!");
                RuleFor(x => x.Status).NotNull().WithMessage("Statuses are required!")
                    .IsInEnum().WithMessage("Statu value is not valid!");
                RuleFor(x => x.StoreName).NotNull().WithMessage("StoreName is required!");
                RuleFor(x => x.CreatedOn)
                    .NotNull().WithMessage("CreatedOn is required!");
            }
        }
    }

    public class OrderCollectionValidator : AbstractValidator<List<Order>>
    {
        public OrderCollectionValidator()
        {
            RuleForEach(x => x).SetValidator(new InsertOrdersValidator());
        }
    }
}
