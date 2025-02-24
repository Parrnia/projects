using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Commands.UpdateReturnOrderTotalDocument;
public class UpdateReturnOrderTotalDocumentCommandValidator : AbstractValidator<UpdateReturnOrderTotalDocumentCommand>
{
    public UpdateReturnOrderTotalDocumentCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.Image)
            .NotEmpty().WithMessage("تصویر اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("توضیحات اجباریست");
        RuleFor(v => v.ReturnOrderTotalId)
            .NotEmpty().WithMessage("شناسه هزینه جانبی اجباریست");
        RuleFor(v => v.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه سفارش بازگشت اجباریست");
    }
}
