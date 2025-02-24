using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice.GetCustomerTickets;
public class GetCustomerTicketsByCustomerIdQueryValidator : AbstractValidator<GetCustomerTicketsByCustomerIdQuery>
{
    public GetCustomerTicketsByCustomerIdQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}