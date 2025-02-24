using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.ProductsCluster.Tags.Commands.UpdateTag;
public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;
    public UpdateTagCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.EnTitle)
            .MustAsync(BeUniqueEnTitle).WithMessage("تگی با این عنوان لاتین موجود است")
            .NotEmpty().WithMessage("عنوان لاتین اجباریست");
        RuleFor(v => v.FaTitle)
            .MustAsync(BeUniqueFaTitle).WithMessage("تگی با این عنوان فارسی موجود است")
            .NotEmpty().WithMessage("عنوان فارسی اجباریست");
    }

    public async Task<bool> BeUniqueEnTitle(string enTitle, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .Where(l => l.Id != _id)
            .AllAsync(l => l.EnTitle != enTitle, cancellationToken);
    }
    public async Task<bool> BeUniqueFaTitle(string faTitle, CancellationToken cancellationToken)
    {
        return await _context.Tags
            .Where(l => l.Id != _id)
            .AllAsync(l => l.FaTitle != faTitle, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}