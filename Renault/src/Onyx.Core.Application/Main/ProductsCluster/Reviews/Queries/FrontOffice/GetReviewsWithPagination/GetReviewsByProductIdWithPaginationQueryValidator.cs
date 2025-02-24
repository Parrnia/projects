using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.FrontOffice.GetReviewsWithPagination;
public class GetReviewsByProductIdWithPaginationQueryValidator : AbstractValidator<GetReviewsByProductIdWithPaginationQuery>
{
    public GetReviewsByProductIdWithPaginationQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}