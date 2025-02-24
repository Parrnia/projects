using FluentValidation;

namespace Onyx.Application.Main.CustomerSupportCluster.CustomerTickets.Queries.Export.ExportExcelCustomerTickets;
public class ExportExcelCustomerTicketsQueryValidator : AbstractValidator<ExportExcelCustomerTicketsQuery>
{
    public ExportExcelCustomerTicketsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}