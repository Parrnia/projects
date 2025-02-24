using FluentValidation;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Queries.Export.ExportExcelTeamMembers;
public class ExportExcelTeamMembersQueryValidator : AbstractValidator<ExportExcelTeamMembersQuery>
{
    public ExportExcelTeamMembersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}