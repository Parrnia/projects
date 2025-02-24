using FluentValidation;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Queries.FrontOffice.GetTeamMembersWithPagination;
public class GetTeamMembersWithPaginationQueryValidator : AbstractValidator<GetTeamMembersWithPaginationQuery>
{
    public GetTeamMembersWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}