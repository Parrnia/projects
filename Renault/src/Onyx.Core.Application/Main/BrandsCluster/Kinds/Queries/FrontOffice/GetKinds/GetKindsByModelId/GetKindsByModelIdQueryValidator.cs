using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.FrontOffice.GetKinds.GetKindsByModelId;
public class GetKindsByModelIdQueryValidator : AbstractValidator<GetKindsByModelIdQuery>
{
    public GetKindsByModelIdQueryValidator()
    {
        RuleFor(x => x.ModelId)
            .NotEmpty().WithMessage("شناسه مدل اجباریست");
    }
}
