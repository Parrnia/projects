using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.FrontOffice.GetReviews.GetReviewsByProductId;
public class GetReviewsByProductIdQueryValidator : AbstractValidator<GetReviewsByProductIdQuery>
{
    public GetReviewsByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
    }
}