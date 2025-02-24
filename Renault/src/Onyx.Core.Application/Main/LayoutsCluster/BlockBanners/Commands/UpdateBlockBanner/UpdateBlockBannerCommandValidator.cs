using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.LayoutsCluster.BlockBanners.Commands.UpdateBlockBanner;
public class UpdateBlockBannerCommandValidator : AbstractValidator<UpdateBlockBannerCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;
    public UpdateBlockBannerCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("عنوان اجباریست");
        RuleFor(v => v.Subtitle)
            .NotEmpty().WithMessage("رنگ دکمه اولیه اجباریست");
        RuleFor(v => v.ButtonText)
            .NotEmpty().WithMessage("متن دکمه اجباریست");
        RuleFor(v => v.Image)
            .NotEmpty().WithMessage("شناسه تصویر اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.BlockBannerPosition)
            .MustAsync(BeUniqueBlockBannerPosition).WithMessage("بنری با این موقعیت روی صفحه موجود است");
    }

    public async Task<bool> BeUniqueBlockBannerPosition(int blockBannerPosition, CancellationToken cancellationToken)
    {
        return await _context.BlockBanners
            .Where(l => l.Id != _id)
            .AllAsync(l => l.BlockBannerPosition != (BlockBannerPosition)blockBannerPosition, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}
