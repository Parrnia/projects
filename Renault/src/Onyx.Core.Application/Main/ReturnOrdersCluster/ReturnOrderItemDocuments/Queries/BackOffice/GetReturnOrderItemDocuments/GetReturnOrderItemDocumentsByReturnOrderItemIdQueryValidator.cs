using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderItemDocuments.Queries.BackOffice.GetReturnOrderItemDocuments;
public class GetReturnOrderItemDocumentsByReturnOrderItemIdQueryValidator : AbstractValidator<GetReturnOrderItemDocumentsByReturnOrderItemIdQuery>
{
    public GetReturnOrderItemDocumentsByReturnOrderItemIdQueryValidator()
    {
        RuleFor(x => x.ReturnOrderItemId)
            .NotEmpty().WithMessage("شناسه آیتم سفارش بازگشت اجباریست");
    }
}
