using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.FrontOffice.GetModels.GetModelsByFamilyId;
public class GetModelsByFamilyIdQueryValidator : AbstractValidator<GetModelsByFamilyIdQuery>
{
    public GetModelsByFamilyIdQueryValidator()
    {
        RuleFor(x => x.FamilyId)
            .NotEmpty().WithMessage("شناسه خانواده اجباریست");
    }
}
