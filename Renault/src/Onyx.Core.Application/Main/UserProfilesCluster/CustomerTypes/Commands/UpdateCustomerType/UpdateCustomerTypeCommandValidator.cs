using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Commands.UpdateCustomerType;
public class UpdateCustomerTypeCommandValidator : AbstractValidator<UpdateCustomerTypeCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;

    public UpdateCustomerTypeCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.CustomerTypeEnum)
            .NotEmpty().WithMessage("نوع مشتری اجباریست")
            .MustAsync(BeUniqueCustomerTypeEnum).WithMessage("یک دسته بندی مشتری با این نوع مشتری موجود است");
        RuleFor(v => v.DiscountPercent)
            .Must(c => c >= 0).WithMessage("درصد تخفیف اجباریست");
    }

    public async Task<bool> BeUniqueCustomerTypeEnum(int customerTypeEnum, CancellationToken cancellationToken)
    {
        var result = await _context.CustomerTypes
            .Where(l => l.Id != _id)
            .AllAsync(l => l.CustomerTypeEnum != (CustomerTypeEnum) customerTypeEnum, cancellationToken);
        return result;
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}