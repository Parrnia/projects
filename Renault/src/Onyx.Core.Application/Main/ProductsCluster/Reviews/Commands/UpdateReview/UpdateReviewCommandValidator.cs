using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Commands.UpdateReview;
public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Rating)
            .NotEmpty().WithMessage("امتیاز اجباریست");
        RuleFor(v => v.Content)
            .NotEmpty().WithMessage("محتوا اجباریست");
        RuleFor(v => v.AuthorName)
            .NotEmpty().WithMessage("نام مولف اجباریست");
    }
}