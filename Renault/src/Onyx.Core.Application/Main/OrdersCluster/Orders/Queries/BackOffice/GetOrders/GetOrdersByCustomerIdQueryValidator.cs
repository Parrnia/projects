using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.BackOffice.GetOrders;
public class GetOrdersByCustomerIdQueryValidator : AbstractValidator<GetOrdersByCustomerIdQuery>
{
    public GetOrdersByCustomerIdQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}
