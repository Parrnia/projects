using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Commands.DeleteReturnOrderItemDocument;
public class DeleteRangeReturnOrderItemDocumentCommandValidator : AbstractValidator<DeleteRangeReturnOrderItemDocumentCommand>
{
    public DeleteRangeReturnOrderItemDocumentCommandValidator()
    {
        RuleFor(v => v.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه سفارش بازشگت مرتبط اجباریست");
        RuleFor(v => v.ReturnOrderItemGroupId)
            .NotEmpty().WithMessage("شناسه گروه آیتم های سفارش بازشگت مرتبط اجباریست");
        RuleFor(v => v.ReturnOrderItemId)
            .NotEmpty().WithMessage("شناسه آیتم سفارش بازشگت مرتبط اجباریست");
    }
}
