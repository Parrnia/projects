using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Commands.DeleteReturnOrderTotalDocument;
public class DeleteReturnOrderTotalDocumentCommandValidator : AbstractValidator<DeleteReturnOrderTotalDocumentCommand>
{
    public DeleteReturnOrderTotalDocumentCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه سفارش بازشگت مرتبط اجباریست");
        RuleFor(v => v.ReturnOrderTotalId)
            .NotEmpty().WithMessage("شناسه هزینه جانبی سفارش بازشگت مرتبط اجباریست");
    }
}