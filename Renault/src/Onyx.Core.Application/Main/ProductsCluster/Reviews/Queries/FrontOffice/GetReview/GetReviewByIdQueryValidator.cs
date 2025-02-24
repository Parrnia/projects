using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.FrontOffice.GetReview;
public class GetReviewByIdQueryValidator : AbstractValidator<GetReviewByIdQuery>
{
    public GetReviewByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}