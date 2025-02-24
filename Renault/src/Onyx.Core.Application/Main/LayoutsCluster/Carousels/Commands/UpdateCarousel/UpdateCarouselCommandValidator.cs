using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.LayoutsCluster.Carousels.Commands.UpdateCarousel;
public class UpdateCarouselCommandValidator : AbstractValidator<UpdateCarouselCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;
    public UpdateCarouselCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
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
            .Where(l => l.Id != _id)
            .AllAsync(l => l.Title != title, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}
