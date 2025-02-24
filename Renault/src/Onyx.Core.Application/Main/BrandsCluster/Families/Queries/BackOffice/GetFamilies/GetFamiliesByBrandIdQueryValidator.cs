using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.BackOffice.GetFamilies;
public class GetFamiliesByBrandIdQueryValidator : AbstractValidator<GetFamiliesByBrandIdQuery>
{
    public GetFamiliesByBrandIdQueryValidator()
    {
        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("شناسه برند اجباریست");
    }
}