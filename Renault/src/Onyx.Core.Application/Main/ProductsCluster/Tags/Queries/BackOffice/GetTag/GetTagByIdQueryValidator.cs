using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Tags.Queries.BackOffice.GetTag;
public class GetTagByIdQueryValidator : AbstractValidator<GetTagByIdQuery>
{
    public GetTagByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}