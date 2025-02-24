using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.BackOffice.GetKinds;
public class GetKindsByModelIdQueryValidator : AbstractValidator<GetKindsByModelIdQuery>
{
    public GetKindsByModelIdQueryValidator()
    {
        RuleFor(x => x.ModelId)
            .NotEmpty().WithMessage("شناسه مدل اجباریست");
    }
}
