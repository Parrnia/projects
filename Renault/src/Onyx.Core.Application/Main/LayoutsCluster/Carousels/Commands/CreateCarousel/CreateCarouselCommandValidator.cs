using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.LayoutsCluster.Carousels.Commands.CreateCarousel;
public class CreateCarouselCommandValidator : AbstractValidator<CreateCarouselCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCarouselCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        
        RuleFor(v => v.Url)
            .NotEmpty().WithMessage("آدرس اجباریست");
        RuleFor(v => v.DesktopImage)
            .NotEmpty().WithMessage("شناسه تصویر دسکتاپ فرد اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.MobileImage)
            .NotEmpty().WithMessage("شناسه تصویر موبایل فرد اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.Offer)
            .NotEmpty().WithMessage("تخفیف اجباریست");
        RuleFor(v => v.Title)
            .MustAsync(BeUniqueTitle).WithMessage("اسلایدری با این عنوان موجود است")
            .NotEmpty().WithMessage("عنوان اجباریست");
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
        RuleFor(v => v.ButtonLabel)
            .NotEmpty().WithMessage("برچسب دکمه اجباریست");
        RuleFor(v => v.Order)
            .GreaterThanOrEqualTo(1).WithMessage("ترتیب باید از صفر بزرگتر باشد");
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _context.Carousels
            .AllAsync(l => l.Title != title, cancellationToken);
    }
}
