using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.UserProfilesCluster.Addresses.Commands.UpdateAddress;
public class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;
    private Guid _customerId;

    public UpdateAddressCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .MustAsync(GetCustomerIdForUniqueness)
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("عنوان اجباریست")
            .MustAsync(BeUniqueTitle).WithMessage("آدرسی با این عنوان موجود است");
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
        return await _context.Addresses
            .Where(l => l.Id != _id && l.CustomerId == _customerId)
            .AllAsync(l => l.Title != title, cancellationToken);
    }
    public async Task<bool> BeUniquePostcode(string postcode, CancellationToken cancellationToken)
    {
        return await _context.Addresses
            .Where(l => l.Id != _id && l.CustomerId == _customerId)
            .AllAsync(l => l.Postcode != postcode, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
    public async Task<bool> GetCustomerIdForUniqueness(Guid customerId, CancellationToken cancellationToken)
    {
        this._customerId = customerId;
        return await Task.FromResult(true);
    }
}