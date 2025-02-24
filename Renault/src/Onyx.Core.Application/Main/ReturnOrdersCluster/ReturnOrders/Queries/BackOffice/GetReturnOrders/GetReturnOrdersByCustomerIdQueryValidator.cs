using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrders;
public class GetReturnOrdersByCustomerIdQueryValidator : AbstractValidator<GetReturnOrdersByCustomerIdQuery>
{
    public GetReturnOrdersByCustomerIdQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}
