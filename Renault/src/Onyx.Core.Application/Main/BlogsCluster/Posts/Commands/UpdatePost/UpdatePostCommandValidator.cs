using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BlogsCluster.Posts.Commands.UpdatePost;
public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;

    public UpdatePostCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("عنوان پست اجباریست")
            .MaximumLength(50).WithMessage("عنوان پست نباید بیشتر از 50 کاراکتر باشد")
            .MustAsync(BeUniqueTitle).WithMessage("پستی با این عنوان موجود است");
        RuleFor(v => v.Body)
            .NotEmpty().WithMessage("متن پست اجباریست")
            .MaximumLength(500).WithMessage("متن پست نباید بیشتر از 500 کاراکتر باشد");
        RuleFor(v => v.BlogCategoryId)
            .NotEmpty().WithMessage("شناسه دسته بندی بلاگ اجباریست");
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _context.Posts
            .Where(l => l.Id != _id)
            .AllAsync(l => l.Title != title, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        _id = requestId;
        return Task.FromResult(true);
    }
}