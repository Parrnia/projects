using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice.GetReviews;
public class GetReviewsByCustomerIdQueryValidator : AbstractValidator<GetReviewsByCustomerIdQuery>
{
    public GetReviewsByCustomerIdQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}