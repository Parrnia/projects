using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Queries.BackOffice.GetReturnOrderTotalDocuments;
public class GetReturnOrderTotalDocumentsByReturnOrderTotalIdQueryValidator : AbstractValidator<GetReturnOrderTotalDocumentsByReturnOrderTotalIdQuery>
{
    public GetReturnOrderTotalDocumentsByReturnOrderTotalIdQueryValidator()
    {
        RuleFor(x => x.ReturnOrderTotalId)
            .NotEmpty().WithMessage("شناسه هزینه جانبی سفارش بازگشت اجباریست");
    }
}
