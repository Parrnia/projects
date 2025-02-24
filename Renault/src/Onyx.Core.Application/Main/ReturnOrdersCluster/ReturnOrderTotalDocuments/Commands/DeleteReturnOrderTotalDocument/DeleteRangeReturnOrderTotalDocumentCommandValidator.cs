using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Commands.DeleteReturnOrderTotalDocument;
public class DeleteRangeReturnOrderTotalDocumentCommandValidator : AbstractValidator<DeleteRangeReturnOrderTotalDocumentCommand>
{
    public DeleteRangeReturnOrderTotalDocumentCommandValidator()
    {
        RuleFor(v => v.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه سفارش بازشگت مرتبط اجباریست");
        RuleFor(v => v.ReturnOrderTotalId)
            .NotEmpty().WithMessage("شناسه هزینه جانبی سفارش بازشگت مرتبط اجباریست");
    }
}
