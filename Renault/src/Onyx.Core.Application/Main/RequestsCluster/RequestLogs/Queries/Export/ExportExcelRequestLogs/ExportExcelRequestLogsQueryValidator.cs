using FluentValidation;

namespace Onyx.Application.Main.RequestsCluster.RequestLogs.Queries.Export.ExportExcelRequestLogs;
public class ExportExcelRequestLogsQueryValidator : AbstractValidator<ExportExcelRequestLogsQuery>
{
    public ExportExcelRequestLogsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}