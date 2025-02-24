using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.Export.ExportExcelCanceledReturnOrders;
public class ExportExcelCanceledReturnOrdersQueryValidator : AbstractValidator<ExportExcelCanceledReturnOrdersQuery>
{
    public ExportExcelCanceledReturnOrdersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}