using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BlogsCluster.Comments.Commands.UpdateComment;

public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCommentCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Text)
            .NotEmpty().WithMessage("متن دیدگاه اجباریست")
            .MaximumLength(80).WithMessage("عنوان دیدگاه نباید بیشتر از 50 کاراکتر باشد");

    }
}