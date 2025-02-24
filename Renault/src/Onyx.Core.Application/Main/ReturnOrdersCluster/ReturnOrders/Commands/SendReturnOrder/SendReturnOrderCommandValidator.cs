using FluentValidation;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.ReturnOrderModels;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.SendReturnOrder;
public class SendReturnOrderCommandValidator : AbstractValidator<SendReturnOrderCommand>
{
    public SendReturnOrderCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Details)
            .NotEmpty().WithMessage("جزئیات اجباریست");
        RuleFor(v => v.ReturnOrderTransportationType)
            .NotEmpty().WithMessage("شیوه بازگشت کالا اجباریست");

        RuleFor(v => v.ReturnShippingPrice != null ? v.DocumentCommandForTotals.FirstOrDefault() : new DocumentCommandForTotal())
            .NotEmpty().WithMessage("مستندات اجباریست");
        RuleFor(v => v.ReturnShippingPrice != null ? v.DocumentCommandForTotals.Select(t => t.Description) : new List<string>{"1"})
            .NotEmpty().WithMessage("توضیحات  مستند اجباریست");
        RuleFor(v => v.ReturnShippingPrice != null ? v.DocumentCommandForTotals.Select(t => t.Image) : new List<Guid>(){Guid.Empty})
            .NotEmpty().WithMessage("تصویر مستند اجباریست");
    }
}
