using FluentValidation;

namespace Onyx.Application.Main.BlogsCluster.Comments.Queries.BackOffice.GetComments;
public class GetCommentsByParentCommentIdQueryValidator : AbstractValidator<GetCommentsByParentCommentIdQuery>
{
    public GetCommentsByParentCommentIdQueryValidator()
    {
        RuleFor(x => x.ParentCommentId)
            .NotEmpty().WithMessage("شناسه دیدگاه اصلی اجباریست");
    }
}