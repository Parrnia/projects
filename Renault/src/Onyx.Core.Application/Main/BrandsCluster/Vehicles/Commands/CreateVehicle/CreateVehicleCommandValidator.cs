using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Commands.CreateVehicle;
public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    private readonly IApplicationDbContext _context;
    private Guid _customerId;
    public CreateVehicleCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.CustomerId)
            .MustAsync(GetCustomerIdForUniqueness)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.VinNumber)
            .MustAsync(BeUniqueVinNumber).WithMessage("خودرویی با این شماره وین موجود است")
            .Must(c => c == null || c?.Length == 17).WithMessage("شماره وین باید دارای 17 کاراکتر باشد");
        RuleFor(v => v.KindId)
            .MustAsync(BeUniqueKindId).WithMessage("خودرویی با این شناسه نوع موجود است")
            .NotEmpty().WithMessage("شناسه نوع اجباریست");
        
    }

    public async Task<bool> BeUniqueVinNumber(string? vinNumber, CancellationToken cancellationToken)
    {
        return vinNumber == null || await _context.Vehicles
            .Where(c => c.CustomerId == _customerId)
            .AllAsync(l => l.VinNumber != vinNumber, cancellationToken);
    }
    public async Task<bool> BeUniqueKindId(int kindId, CancellationToken cancellationToken)
    {
        return await _context.Vehicles
            .Where(c => c.CustomerId == _customerId)
            .AllAsync(l => l.KindId != kindId, cancellationToken);
    }
    public async Task<bool> GetCustomerIdForUniqueness(Guid customerId, CancellationToken cancellationToken)
    {
        this._customerId = customerId;
        return await Task.FromResult(true);
    }
}
