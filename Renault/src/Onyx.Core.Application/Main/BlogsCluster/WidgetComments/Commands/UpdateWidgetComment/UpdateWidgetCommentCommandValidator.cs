using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BlogsCluster.WidgetComments.Commands.UpdateWidgetComment;

public class UpdateWidgetCommentCommandValidator : AbstractValidator<UpdateWidgetCommentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateWidgetCommentCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}