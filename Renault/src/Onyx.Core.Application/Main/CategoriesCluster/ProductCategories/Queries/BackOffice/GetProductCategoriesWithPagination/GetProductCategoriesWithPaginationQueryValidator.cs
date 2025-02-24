using FluentValidation;

namespace Onyx.Application.Main.CategoriesCluster.ProductCategories.Queries.BackOffice.GetProductCategoriesWithPagination;
public class GetProductCategoriesWithPaginationQueryValidator : AbstractValidator<GetProductCategoriesWithPaginationQuery>
{
    public GetProductCategoriesWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}