using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Models.Commands.UpdateModel;
public class UpdateModelCommandValidator : AbstractValidator<UpdateModelCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;
    public UpdateModelCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.FamilyId)
            .NotEmpty().WithMessage("شناسه خانواده اجباریست");
        RuleFor(v => v.LocalizedName)
            //.MustAsync(BeUniqueLocalizedName).WithMessage("مدلی با این نام فارسی موجود است")
            .NotEmpty().WithMessage("نام فارسی اجباریست")
            .MaximumLength(250).WithMessage("نام فارسی نباید بیشتر از 250 کاراکتر باشد");
        RuleFor(v => v.Name)
            //.MustAsync(BeUniqueName).WithMessage("مدلی با این نام لاتین موجود است")
            .MaximumLength(250).WithMessage("نام لاتین نباید بیشتر از 250 کاراکتر باشد")
            .NotEmpty().WithMessage("نام لاتین اجباریست");
    }

    public async Task<bool> BeUniqueLocalizedName(string localizedName, CancellationToken cancellationToken)
    {
        return await _context.Models
            .Where(l => l.Id != _id)
            .AllAsync(l => l.LocalizedName != localizedName, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Models
            .Where(l => l.Id != _id)
            .AllAsync(l => l.Name != name, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}
