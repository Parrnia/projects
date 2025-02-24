using FluentValidation;

namespace Onyx.Application.Main.OrdersCluster.Orders.Queries.FrontOffice.GetInvoice;
public class GetOrderInvoiceQueryValidator : AbstractValidator<GetOrderInvoiceQuery>
{
    public GetOrderInvoiceQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}