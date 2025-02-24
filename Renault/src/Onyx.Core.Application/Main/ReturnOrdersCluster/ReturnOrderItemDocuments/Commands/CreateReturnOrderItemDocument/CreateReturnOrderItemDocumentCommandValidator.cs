using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Commands.CreateReturnOrderItemDocument;
public class CreateReturnOrderItemDocumentCommandValidator : AbstractValidator<CreateReturnOrderItemDocumentCommand>
{
    public CreateReturnOrderItemDocumentCommandValidator()
    {
        RuleFor(v => v.Image)
            .NotEmpty().WithMessage("تصویر اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("توضیحات اجباریست");
        RuleFor(v => v.ReturnOrderItemId)
            .NotEmpty().WithMessage("شناسه آیتم اجباریست");
        RuleFor(v => v.ReturnOrderItemGroupId)
            .NotEmpty().WithMessage("شناسه گروه آیتم اجباریست");
        RuleFor(v => v.ReturnOrderId)
            .NotEmpty().WithMessage("شناسه سفارش بازگشت اجباریست");
    }
}
