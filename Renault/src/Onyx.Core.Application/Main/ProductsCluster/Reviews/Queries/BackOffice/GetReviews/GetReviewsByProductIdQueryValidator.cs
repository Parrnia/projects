using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice.GetReviews;
public class GetReviewsByProductIdQueryValidator : AbstractValidator<GetReviewsByProductIdQuery>
{
    public GetReviewsByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
    }
}