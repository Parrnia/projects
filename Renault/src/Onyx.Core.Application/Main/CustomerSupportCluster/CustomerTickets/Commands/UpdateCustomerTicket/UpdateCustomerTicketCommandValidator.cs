using FluentValidation;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Commands.UpdateCustomerTicket;
public class UpdateCustomerTicketCommandValidator : AbstractValidator<UpdateCustomerTicketCommand>
{
    public UpdateCustomerTicketCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Subject)
            .NotEmpty().WithMessage("موضوع اجباریست")
            .MaximumLength(50).WithMessage("موضوع نباید بیشتر از 50 کاراکتر باشد");
        RuleFor(v => v.Message)
            .NotEmpty().WithMessage("پیام اجباریست")
            .MaximumLength(1000).WithMessage("پیام نباید بیشتر از 1000 کاراکتر باشد");
    }
}