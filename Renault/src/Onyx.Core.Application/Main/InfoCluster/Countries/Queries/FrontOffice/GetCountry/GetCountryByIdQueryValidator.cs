using FluentValidation;

namespace Onyx.Application.Main.InfoCluster.Countries.Queries.FrontOffice.GetCountry;
public class GetCountryByIdQueryValidator : AbstractValidator<GetCountryByIdQuery>
{
    public GetCountryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}