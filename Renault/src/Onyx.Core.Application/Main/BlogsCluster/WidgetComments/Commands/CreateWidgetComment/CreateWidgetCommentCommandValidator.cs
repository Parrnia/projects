using FluentValidation;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.BlogsCluster.WidgetComments.Commands.CreateWidgetComment;
public class CreateWidgetCommentCommandValidator : AbstractValidator<CreateWidgetCommentCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateWidgetCommentCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.AuthorId)
            .NotEmpty().WithMessage("شناسه مولف اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}
