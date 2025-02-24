using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.UpdateOrder;
public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;

    public UpdateOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        //RuleFor(v => v.PaymentType)
        //    .IsInEnum().WithMessage("Paymentنوع اجباریست");
        RuleFor(v => v.AddressDetails1)
            .NotEmpty().WithMessage("جزئیات اول آدرس ارسال کالا اجباریست");
        RuleFor(v => v.AddressDetails2)
            .NotEmpty().WithMessage("جزئیات دوم آدرس ارسال کالا اجباریست");
        RuleFor(v => v.Postcode)
            .NotEmpty().WithMessage("کد پستی آدرس ارسال کالا اجباریست");
        RuleFor(v => v.PhoneNumber)
            .NotEmpty().WithMessage("شماره تماس اجباریست");
        RuleFor(v => v.FirstName)
            .NotEmpty().WithMessage("نام مشتری اجباریست");
        RuleFor(v => v.LastName)
            .NotEmpty().WithMessage("نام خانوادگی مشتری اجباریست");
    }

    public async Task<bool> BeUniqueNumber(string number, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Where(l => l.Id != _id)
            .AllAsync(l => l.Number != number, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}
