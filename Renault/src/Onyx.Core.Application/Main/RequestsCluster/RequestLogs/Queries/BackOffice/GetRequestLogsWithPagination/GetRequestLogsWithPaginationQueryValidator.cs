using FluentValidation;

namespace Onyx.Application.Main.RequestsCluster.RequestLogs.Queries.BackOffice.GetRequestLogsWithPagination;
public class GetRequestLogsWithPaginationQueryValidator : AbstractValidator<GetRequestLogsWithPaginationQuery>
{
    public GetRequestLogsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}