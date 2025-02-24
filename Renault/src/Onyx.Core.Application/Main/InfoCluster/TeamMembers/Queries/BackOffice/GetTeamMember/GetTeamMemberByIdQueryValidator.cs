using FluentValidation;

namespace Onyx.Application.Main.InfoCluster.TeamMembers.Queries.BackOffice.GetTeamMember;
public class GetTeamMemberByIdQueryValidator : AbstractValidator<GetTeamMemberByIdQuery>
{
    public GetTeamMemberByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}