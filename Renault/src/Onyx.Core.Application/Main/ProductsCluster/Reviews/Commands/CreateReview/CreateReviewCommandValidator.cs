using FluentValidation;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Commands.CreateReview;
public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateReviewCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Rating)
            .NotEmpty().WithMessage("امتیاز اجباریست");
        RuleFor(v => v.Content)
            .NotEmpty().WithMessage("محتوا اجباریست");
        RuleFor(v => v.AuthorName)
            .NotEmpty().WithMessage("نام مولف اجباریست");
        RuleFor(v => v.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
        RuleFor(v => v.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}
