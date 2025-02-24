using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Commands.CreateAddress;
public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
{
    private readonly IApplicationDbContext _context;
    private Guid? _customerId;
    public CreateAddressCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .MustAsync(GetCustomerIdForUniqueness);
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("عنوان اجباریست")
            .MustAsync(BeUniqueTitle).WithMessage("آدرسی با این عنوان موجود است");
        RuleFor(v => v.CountryId)
            .NotEmpty().WithMessage("شناسه کشور اجباریست");
        RuleFor(v => v.AddressDetails1)
            .NotEmpty().WithMessage("جزئیات آدرس اجباریست");
        RuleFor(v => v.City)
            .NotEmpty().WithMessage("شهر اجباریست");
        RuleFor(v => v.State)
            .NotEmpty().WithMessage("استان اجباریست");
        RuleFor(v => v.Postcode)
            .NotEmpty().WithMessage("کد پستی اجباریست")
            .MustAsync(BeUniquePostcode).WithMessage("آدرسی با این کد پستی موجود است");
        RuleFor(v => v.Default.ToString())
            .NotEmpty().WithMessage("پیشفرض بودن اجباریست");
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        var result = _customerId == null || await _context.Addresses
            .Where(l => l.CustomerId == _customerId)
            .AllAsync(l => l.Title != title, cancellationToken);
        return result;
    }
    public async Task<bool> BeUniquePostcode(string postcode, CancellationToken cancellationToken)
    {
        var result = _customerId == null || await _context.Addresses
            .Where(l => l.CustomerId == _customerId)
            .AllAsync(l => l.Postcode != postcode, cancellationToken);
        return result;
    }
    public async Task<bool> GetCustomerIdForUniqueness(Guid customerId, CancellationToken cancellationToken)
    {
        this._customerId = customerId;
        return await Task.FromResult(true);
    }
}
