using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.UserProfilesCluster.Customers.Commands.AddVehicleToCustomer;
public class AddVehicleToCustomerCommandValidator : AbstractValidator<AddVehicleToCustomerCommand>
{
    private readonly IApplicationDbContext _context;
    private Guid _id;
    public AddVehicleToCustomerCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.CustomerId)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.VehicleId)
            .MustAsync(PreventDuplicateVehicle).WithMessage("خودرویی با این شناسه خودرو برای این مشتری موجود است");
        RuleFor(v => v.KindId)
            .MustAsync(PreventDuplicateKind).WithMessage("خودرویی با این شناسه نوع برای این مشتری موجود است");
    }
    public async Task<bool> PreventDuplicateVehicle(int vehicleId, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers.Include(c => c.Vehicles)
            .SingleOrDefaultAsync(e => e.Id == _id, cancellationToken);
        return entity != null && !entity.Vehicles.Select(c => c.Id).Contains(vehicleId);
    }

    public async Task<bool> PreventDuplicateKind(int kindId, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers.Include(c => c.Vehicles)
            .SingleOrDefaultAsync(e => e.Id == _id, cancellationToken);
        return entity != null && !entity.Vehicles.Select(c => c.KindId).Contains(kindId);
    }

    public Task<bool> GetIdForUniqueness(Guid requestId, CancellationToken cancellationToken)
    {
        _id = requestId;
        return Task.FromResult(true);
    }
}