using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.CountingUnits.Queries.FrontOffice.GetCountingUnit;
public class GetCountingUnitByIdQueryValidator : AbstractValidator<GetCountingUnitByIdQuery>
{
    public GetCountingUnitByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}