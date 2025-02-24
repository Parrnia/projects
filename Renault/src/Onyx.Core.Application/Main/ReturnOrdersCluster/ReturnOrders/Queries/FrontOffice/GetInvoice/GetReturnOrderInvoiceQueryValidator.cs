using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.FrontOffice.GetInvoice;
public class GetReturnOrderInvoiceQueryValidator : AbstractValidator<GetReturnOrderInvoiceQuery>
{
    public GetReturnOrderInvoiceQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}