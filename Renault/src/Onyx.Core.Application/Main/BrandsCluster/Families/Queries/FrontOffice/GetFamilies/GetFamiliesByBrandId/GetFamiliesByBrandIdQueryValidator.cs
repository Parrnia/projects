using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.FrontOffice.GetFamilies.GetFamiliesByBrandId;
public class GetFamiliesByBrandIdQueryValidator : AbstractValidator<GetFamiliesByBrandIdQuery>
{
    public GetFamiliesByBrandIdQueryValidator()
    {
        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("شناسه برند اجباریست");
    }
}