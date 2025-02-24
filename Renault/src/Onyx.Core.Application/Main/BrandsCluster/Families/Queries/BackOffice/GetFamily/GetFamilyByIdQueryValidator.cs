using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.BackOffice.GetFamily;
public class GetFamilyByIdQueryValidator : AbstractValidator<GetFamilyByIdQuery>
{
    public GetFamilyByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}