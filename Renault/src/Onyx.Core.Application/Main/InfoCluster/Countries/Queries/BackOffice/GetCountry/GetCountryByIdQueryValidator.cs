using FluentValidation;

namespace Onyx.Application.Main.InfoCluster.Countries.Queries.BackOffice.GetCountry;
public class GetCountryByIdQueryValidator : AbstractValidator<GetCountryByIdQuery>
{
    public GetCountryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}