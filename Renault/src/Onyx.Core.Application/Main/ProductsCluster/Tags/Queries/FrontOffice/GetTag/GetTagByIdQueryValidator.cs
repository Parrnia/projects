using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Tags.Queries.FrontOffice.GetTag;
public class GetTagByIdQueryValidator : AbstractValidator<GetTagByIdQuery>
{
    public GetTagByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}