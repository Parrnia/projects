using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.CountingUnitTypes.Queries.BackOffice.GetCountingUnitType;
public class GetCountingUnitTypeByIdQueryValidator : AbstractValidator<GetCountingUnitTypeByIdQuery>
{
    public GetCountingUnitTypeByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}