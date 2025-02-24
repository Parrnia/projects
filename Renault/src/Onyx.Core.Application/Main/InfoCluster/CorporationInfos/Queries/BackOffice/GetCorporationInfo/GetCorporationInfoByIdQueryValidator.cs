using FluentValidation;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Queries.BackOffice.GetCorporationInfo;
public class GetCorporationInfoByIdQueryValidator : AbstractValidator<GetCorporationInfoByIdQuery>
{
    public GetCorporationInfoByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}