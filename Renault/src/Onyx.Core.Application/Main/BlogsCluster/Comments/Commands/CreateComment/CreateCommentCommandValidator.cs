using FluentValidation;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.BlogsCluster.Comments.Commands.CreateComment;
public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateCommentCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Text)
            .NotEmpty().WithMessage("عنوان دیدگاه اجباریست")
            .MaximumLength(80).WithMessage("عنوان دیدگاه نباید بیشتر از 50 کاراکتر باشد");
        RuleFor(v => v.AuthorId)
            .NotEmpty().WithMessage("شناسه مولف دیدگاه اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.PostId)
            .NotEmpty().WithMessage("شناسه پست مربوط به دیدگاه اجباریست");
    }
}
