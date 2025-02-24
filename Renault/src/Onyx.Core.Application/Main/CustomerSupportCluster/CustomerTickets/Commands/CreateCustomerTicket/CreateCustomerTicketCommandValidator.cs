using FluentValidation;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Commands.CreateCustomerTicket;
public class CreateCustomerTicketCommandValidator : AbstractValidator<CreateCustomerTicketCommand>
{
    private readonly IApplicationDbContext _context;
    public CreateCustomerTicketCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Subject)
            .NotEmpty().WithMessage("موضوع اجباریست")
            .MaximumLength(50).WithMessage("موضوع نباید بیشتر از 50 کاراکتر باشد");
        RuleFor(v => v.Message)
            .NotEmpty().WithMessage("پیام اجباریست")
            .MaximumLength(1000).WithMessage("پیام نباید بیشتر از 1000 کاراکتر باشد");
        RuleFor(v => v.CustomerPhoneNumber)
            .NotEmpty().WithMessage("شماره تماس مشتری اجباریست");
        RuleFor(v => v.CustomerName)
            .NotEmpty().WithMessage("نام مشتری اجباریست");
        RuleFor(v => v.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}
