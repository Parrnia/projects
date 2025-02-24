using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Queries.Export.ExportExcelCostRefundedReturnOrders;
public class ExportExcelCostRefundedReturnOrdersQueryValidator : AbstractValidator<ExportExcelCostRefundedReturnOrdersQuery>
{
    public ExportExcelCostRefundedReturnOrdersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}