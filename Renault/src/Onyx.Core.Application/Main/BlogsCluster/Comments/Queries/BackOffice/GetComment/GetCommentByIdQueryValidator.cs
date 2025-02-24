using FluentValidation;

namespace Onyx.Application.Main.BlogsCluster.Comments.Queries.BackOffice.GetComment;
public class GetCommentByIdQueryValidator : AbstractValidator<GetCommentByIdQuery>
{
    public GetCommentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}