using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.LayoutsCluster.Themes.Commands.CreateTheme;
public class CreateThemeCommandValidator : AbstractValidator<CreateThemeCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateThemeCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        
        RuleFor(v => v.Title)
            .MustAsync(BeUniqueTitle).WithMessage("قالبی با این عنوان موجود است")
            .NotEmpty().WithMessage("عنوان اجباریست");
        RuleFor(v => v.BtnPrimaryColor)
            .NotEmpty().WithMessage("رنگ دکمه اولیه اجباریست");
        RuleFor(v => v.BtnPrimaryHoverColor)
            .NotEmpty().WithMessage("رنگ هاور دکمه اولیه اجباریست");
        RuleFor(v => v.BtnSecondaryColor)
            .NotEmpty().WithMessage("رنگ دکمه ثانویه اجباریست");
        RuleFor(v => v.BtnSecondaryHoverColor)
            .NotEmpty().WithMessage("رنگ هاور دکمه ثانویه اجباریست");
        RuleFor(v => v.ThemeColor)
            .NotEmpty().WithMessage("رنگ قالب اجباریست");
        RuleFor(v => v.HeaderAndFooterColor)
            .NotEmpty().WithMessage("رنگ هدر و فوتر اجباریست");
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _context.Themes
            .AllAsync(l => l.Title != title, cancellationToken);
    }
}
