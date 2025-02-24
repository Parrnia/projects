using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.BackOffice.GetReturnOrdersWithPagination;
public class GetSentReturnOrdersWithPaginationQueryValidator : AbstractValidator<GetSentReturnOrdersWithPaginationQuery>
{
    public GetSentReturnOrdersWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}