using FluentValidation;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.BackOffice.GetCustomerTicket;
public class GetCustomerTicketByIdQueryValidator : AbstractValidator<GetCustomerTicketByIdQuery>
{
    public GetCustomerTicketByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}