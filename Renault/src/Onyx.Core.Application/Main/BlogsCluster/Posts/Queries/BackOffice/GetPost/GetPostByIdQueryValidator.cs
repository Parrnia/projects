using FluentValidation;

namespace Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice.GetPost;
public class GetPostByIdQueryValidator : AbstractValidator<GetPostByIdQuery>
{
    public GetPostByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}