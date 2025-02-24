using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrderTotalDocuments.Queries.Export.ExportExcelReturnOrderTotalDocuments;
public class ExportExcelReturnOrderTotalDocumentsQueryValidator : AbstractValidator<ExportExcelReturnOrderTotalDocumentsQuery>
{
    public ExportExcelReturnOrderTotalDocumentsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}