using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.FrontOffice.GetProductsWithPagination.GetRelatedProductsWithPagination;
public class GetRelatedProductsWithPaginationQueryValidator : AbstractValidator<GetRelatedProductsWithPaginationQuery>
{
    public GetRelatedProductsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
        RuleFor(x => x.CustomerTypeEnum)
            .NotEmpty().WithMessage("نوع مشتری اجباریست");
    }
}
