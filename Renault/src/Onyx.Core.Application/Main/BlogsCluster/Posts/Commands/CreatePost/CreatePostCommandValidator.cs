using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.BlogsCluster.Posts.Commands.CreatePost;
public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    private readonly IApplicationDbContext _context;
    public CreatePostCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("عنوان پست اجباریست")
            .MaximumLength(50).WithMessage("عنوان پست نباید بیشتر از 50 کاراکتر باشد")
            .MustAsync(BeUniqueTitle).WithMessage("پستی با این عنوان موجود است");
        RuleFor(v => v.Body)
            .NotEmpty().WithMessage("متن پست اجباریست")
            .MaximumLength(500).WithMessage("متن پست نباید بیشتر از 500 کاراکتر باشد");
        RuleFor(v => v.BlogCategoryId)
            .NotEmpty().WithMessage("شناسه دسته بندی بلاگ اجباریست");
        RuleFor(v => v.AuthorId)
            .NotEmpty().WithMessage("شناسه مولف اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _context.Posts
            .AllAsync(l => l.Title != title, cancellationToken);
    }
}
