using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.BrandsCluster.Vehicles.Commands.UpdateVehicle;
public class UpdateVehicleCommandValidator : AbstractValidator<UpdateVehicleCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;
    private Guid _customerId;

    public UpdateVehicleCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.CustomerId)
            .MustAsync(GetCustomerIdForUniqueness)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.VinNumber)
            .MustAsync(BeUniqueVinNumber).WithMessage("خودرویی با این شماره وین موجود است")
            .Must(c => c == null || c?.Length == 17).WithMessage("شماره وین نباید بیشتر از 17 کاراکتر باشد");
        RuleFor(v => v.KindId)
            .MustAsync(BeUniqueKindId).WithMessage("خودرویی با این شناسه نوع موجود است")
            .NotEmpty().WithMessage("شناسه نوع اجباریست");
    }

    public async Task<bool> BeUniqueVinNumber(string? vinNumber, CancellationToken cancellationToken)
    {
        return await _context.Vehicles
            .Where(c => c.CustomerId == _customerId && c.Id != _id)
            .AllAsync(l => l.VinNumber != vinNumber, cancellationToken);
    }
    public async Task<bool> BeUniqueKindId(int kindId, CancellationToken cancellationToken)
    {
        return await _context.Vehicles
            .Where(c => c.CustomerId == _customerId && c.Id != _id)
            .AllAsync(l => l.KindId != kindId, cancellationToken);
    }
    public async Task<bool> GetCustomerIdForUniqueness(Guid customerId, CancellationToken cancellationToken)
    {
        this._customerId = customerId;
        return await Task.FromResult(true);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}