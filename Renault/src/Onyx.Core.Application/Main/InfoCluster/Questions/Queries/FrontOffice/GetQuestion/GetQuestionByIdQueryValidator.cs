using FluentValidation;

namespace Onyx.Application.Main.InfoCluster.Questions.Queries.FrontOffice.GetQuestion;
public class GetQuestionByIdQueryValidator : AbstractValidator<GetQuestionByIdQuery>
{
    public GetQuestionByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}