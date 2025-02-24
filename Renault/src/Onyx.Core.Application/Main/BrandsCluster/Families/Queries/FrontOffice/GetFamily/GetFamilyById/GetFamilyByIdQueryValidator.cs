using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.FrontOffice.GetFamily.GetFamilyById;
public class GetFamilyByIdQueryValidator : AbstractValidator<GetFamilyByIdQuery>
{
    public GetFamilyByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}