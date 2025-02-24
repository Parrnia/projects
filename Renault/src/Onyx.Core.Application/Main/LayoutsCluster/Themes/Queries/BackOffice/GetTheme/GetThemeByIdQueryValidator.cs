using FluentValidation;

namespace Onyx.Application.Main.LayoutsCluster.Themes.Queries.BackOffice.GetTheme;
public class GetThemeByIdQueryValidator : AbstractValidator<GetThemeByIdQuery>
{
    public GetThemeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}