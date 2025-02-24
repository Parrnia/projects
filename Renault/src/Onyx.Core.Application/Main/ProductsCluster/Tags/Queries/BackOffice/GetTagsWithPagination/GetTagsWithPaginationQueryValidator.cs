using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Tags.Queries.BackOffice.GetTagsWithPagination;
public class GetTagsWithPaginationQueryValidator : AbstractValidator<GetTagsWithPaginationQuery>
{
    public GetTagsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}