using FluentValidation;

namespace Onyx.Application.Main.BlogsCluster.Comments.Queries.BackOffice.GetComments;
public class GetCommentsByPostIdQueryValidator : AbstractValidator<GetCommentsByPostIdQuery>
{
    public GetCommentsByPostIdQueryValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty().WithMessage("شناسه پست اجباریست");
    }
}