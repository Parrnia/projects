using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.OrdersCluster.Orders.Commands.CreateOrder.CreateOrder;
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    private readonly IApplicationDbContext _context;
    private CustomerTypeEnum _customerTypeEnum;


    public CreateOrderCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.CustomerTypeEnum)
            .MustAsync(GetCustomerType)
            .IsInEnum()
            .WithMessage("نوع مشتری اجباریست");
        //RuleFor(v => v.Number)
        //    .MustAsync(BeUniqueNumber).WithMessage("سفارشی با این شماره موجود است")
        //    .NotEmpty().WithMessage("شماره اجباریست");
        RuleFor(v => v.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.Quantity)
            .NotEmpty().WithMessage("تعداد اجباریست");
        RuleFor(v => v.Subtotal)
            .NotEmpty().WithMessage("جمع قیمت کل محصولات اجباریست");
        RuleFor(v => v.Total)
            .NotEmpty().WithMessage("مبلغ پرداختی اجباریست");
        RuleFor(v => v.PhoneNumber)
            .NotEmpty().WithMessage("شماره تماس اجباریست");
        RuleFor(v => v.FirstName)
            .NotEmpty().WithMessage("نام مشتری اجباریست");
        RuleFor(v => v.LastName)
            .NotEmpty().WithMessage("نام خانوادگی مشتری اجباریست");
        RuleFor(v => v.NationalCode)
            .NotEmpty().WithMessage("کد ملی مشتری اجباریست");


        RuleFor(v => v.OrderItems.Select(c => c.Quantity))
            .NotEmpty().WithMessage("تعداد اجباریست");
        RuleFor(v => v.OrderItems.Select(c => c.ProductId))
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
        RuleFor(v => v.OrderItems.Select(c => c.ProductAttributeOptionId))
            .NotEmpty().WithMessage("شناسه نوع آپشن محصول اجباریست");
        RuleFor(v => v.OrderItems.Select(c => new ForAvailability(){ ProductAttributeOptionId = c.ProductAttributeOptionId, Quantity = c.Quantity }).ToList())
            .MustAsync(CheckAvailability).WithMessage("تعداد درخواستی از تعداد موجود در انبار بیشتر است");

        RuleFor(v => v.OrderItems.Select(c => c.OrderItemOptions.Select(e => e.Name)))
            .NotEmpty().WithMessage("نام لاتین اجباریست");
        RuleFor(v => v.OrderItems.Select(c => c.OrderItemOptions.Select(e => e.Value)))
            .NotEmpty().WithMessage("مقدار اجباریست");


        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("عنوان اجباریست");
        RuleFor(v => v.CountryId)
            .NotEmpty().WithMessage("شناسه کشور اجباریست");
        RuleFor(v => v.AddressDetails1)
            .NotEmpty().WithMessage("جزئیات آدرس اجباریست");
        RuleFor(v => v.City)
            .NotEmpty().WithMessage("شهر اجباریست");
        RuleFor(v => v.State)
            .NotEmpty().WithMessage("استان اجباریست");
        RuleFor(v => v.Postcode)
            .NotEmpty().WithMessage("کد پستی اجباریست");
    }

    public async Task<bool> BeUniqueNumber(string number, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .AllAsync(l => l.Number != number, cancellationToken);
    }
    public async Task<bool> GetCustomerType(CustomerTypeEnum customerType, CancellationToken cancellationToken)
    {
        _customerTypeEnum = customerType;
        return await Task.FromResult(true);
    }
    public async Task<bool> CheckAvailability(List<ForAvailability> forMax, CancellationToken cancellationToken)
    {
        var result = true;
        foreach (var item in forMax)
        {
            var productAttributeOption = await this._context
                .ProductAttributeOptions
                .Include(c => c.ProductAttributeOptionRoles)
                .SingleOrDefaultAsync(e => e.Id == item.ProductAttributeOptionId, cancellationToken);
            if (productAttributeOption != null)
            {
                result &= productAttributeOption.ProductAttributeOptionRoles.SingleOrDefault(c => c.CustomerTypeEnum == _customerTypeEnum)?.CurrentMaxOrderQty >= item.Quantity &&
                          productAttributeOption.TotalCount >= item.Quantity &&
                          productAttributeOption?.ProductAttributeOptionRoles.SingleOrDefault(c => c.CustomerTypeEnum == _customerTypeEnum)?.CurrentMinOrderQty <= item.Quantity;
            }
            else
            {
                return false;
            }
        }
        return result;
    }
}

public class ForAvailability
{
    public int ProductAttributeOptionId { get; set; }
    public double Quantity { get; set; }
}

