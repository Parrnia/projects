using FluentValidation;

namespace Onyx.Application.Main.BlogsCluster.WidgetComments.Queries.BackOffice.GetWidgetComment;
public class GetWidgetCommentByIdQueryValidator : AbstractValidator<GetWidgetCommentByIdQuery>
{
    public GetWidgetCommentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}