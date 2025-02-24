using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.BrandsCluster.Models.Commands.CreateModel;
public class CreateModelCommandValidator : AbstractValidator<CreateModelCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateModelCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        
        RuleFor(v => v.LocalizedName)
            //.MustAsync(BeUniqueLocalizedName).WithMessage("مدلی با این نام فارسی موجود است")
            .NotEmpty().WithMessage("نام فارسی اجباریست")
            .MaximumLength(250).WithMessage("نام فارسی نباید بیشتر از 250 کاراکتر باشد");
        RuleFor(v => v.Name)
            //.MustAsync(BeUniqueName).WithMessage("مدلی با این نام لاتین موجود است")
            .MaximumLength(250).WithMessage("نام لاتین نباید بیشتر از 250 کاراکتر باشد")
            .NotEmpty().WithMessage("نام لاتین اجباریست");
        RuleFor(v => v.FamilyId)
            .NotEmpty().WithMessage("شناسه خانواده اجباریست");
    }

    public async Task<bool> BeUniqueLocalizedName(string localizedName, CancellationToken cancellationToken)
    {
        return await _context.Models
            .AllAsync(l => l.LocalizedName != localizedName, cancellationToken);
    }
    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
    {
        return await _context.Models
            .AllAsync(l => l.Name != name, cancellationToken);
    }
    //public async Task<bool> BeUniqueLocalizedNameKind(IList<ForComplexUniqueness> list, CancellationToken cancellationToken)
    //{
    //    if (list.GroupBy(item => item.Value)
    //        .Any(group => group.Count() > 1))
    //    {
    //        return false;
    //    }

    //    var kinds = await _context.Kinds.ToListAsync(cancellationToken);
    //    foreach (var item in list)
    //    {
    //        foreach (var kind in kinds)
    //        {
    //            if (item.Id != kind.Id && item.Value == kind.LocalizedName)
    //            {
    //                return false;
    //            }
    //        }
    //    }
    //    return true;
    //}

    //public async Task<bool> BeUniqueNameKind(IList<ForComplexUniqueness> list, CancellationToken cancellationToken)
    //{
    //    if (list.GroupBy(item => item.Value)
    //        .Any(group => group.Count() > 1))
    //    {
    //        return false;
    //    }

    //    var kinds = await _context.Kinds.ToListAsync(cancellationToken);
    //    foreach (var item in list)
    //    {
    //        foreach (var kind in kinds)
    //        {
    //            if (item.Id != kind.Id && item.Value == kind.Name)
    //            {
    //                return false;
    //            }
    //        }
    //    }
    //    return true;
    //}
}
