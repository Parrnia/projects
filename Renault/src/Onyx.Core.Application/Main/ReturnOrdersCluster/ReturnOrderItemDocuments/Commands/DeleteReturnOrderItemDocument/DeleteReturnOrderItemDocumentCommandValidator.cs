using FluentValidation;
using Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItems.Commands.DeleteReturnOrderItem;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Commands.DeleteReturnOrderItemDocument;
public class DeleteReturnOrderItemDocumentCommandValidator : AbstractValidator<DeleteReturnOrderItemDocumentCommand>
{
    public DeleteReturnOrderItemDocumentCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه سفارش بازشگت مرتبط اجباریست");
        RuleFor(v => v.ReturnOrderItemGroupId)
            .NotEmpty().WithMessage("شناسه گروه آیتم های سفارش بازشگت مرتبط اجباریست");
        RuleFor(v => v.ReturnOrderItemId)
            .NotEmpty().WithMessage("شناسه آیتم سفارش بازشگت مرتبط اجباریست");
    }
}
