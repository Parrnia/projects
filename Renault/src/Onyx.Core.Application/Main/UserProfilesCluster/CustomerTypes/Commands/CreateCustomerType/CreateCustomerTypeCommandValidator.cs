using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.UserProfilesCluster.CustomerTypes.Commands.CreateCustomerType;
public class CreateCustomerTypeCommandValidator : AbstractValidator<CreateCustomerTypeCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateCustomerTypeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.CustomerTypeEnum)
            .NotEmpty().WithMessage("نوع مشتری اجباریست")
            .MustAsync(BeUniqueCustomerTypeEnum).WithMessage("یک دسته بندی مشتری با این نوع مشتری موجود است");
        RuleFor(v => v.DiscountPercent)
            .Must(c => c >= 0).WithMessage("درصد تخفیف اجباریست");
    }

    public async Task<bool> BeUniqueCustomerTypeEnum(int customerTypeEnum, CancellationToken cancellationToken)
    {
        var result = await _context.CustomerTypes
            .AllAsync(l => l.CustomerTypeEnum !=(CustomerTypeEnum) customerTypeEnum, cancellationToken);
        return result;
    }
}
