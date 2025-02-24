using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.FrontOffice.GetModel.GetModelById;
public class GetModelByIdQueryValidator : AbstractValidator<GetModelByIdQuery>
{
    public GetModelByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}