using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.BackOffice.GetProductStatusesWithPagination;
public class GetProductStatusesWithPaginationQueryValidator : AbstractValidator<GetProductStatusesWithPaginationQuery>
{
    public GetProductStatusesWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}