using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetReturnOrdersWithPagination.GetReturnOrdersByCustomerIdWithPagination;
public class GetReturnOrdersByCustomerIdWithPaginationQueryValidator : AbstractValidator<GetReturnOrdersByCustomerIdWithPaginationQuery>
{
    public GetReturnOrdersByCustomerIdWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}