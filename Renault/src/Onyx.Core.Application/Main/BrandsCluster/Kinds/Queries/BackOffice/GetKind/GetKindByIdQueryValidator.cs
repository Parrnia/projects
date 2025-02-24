using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.BackOffice.GetKind;
public class GetKindByIdQueryValidator : AbstractValidator<GetKindByIdQuery>
{
    public GetKindByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}