using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Questions.Commands.CreateQuestion;
public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateQuestionCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.QuestionText)
            .MustAsync(BeUniqueQuestion).WithMessage("سوالی با این متن موجود است")
            .NotEmpty().WithMessage("متن سوال اجباریست");
        RuleFor(v => v.AnswerText)
            .NotEmpty().WithMessage("متن پاسخ اجباریست");
        RuleFor(v => v.QuestionType)
            .Must(c => c >= 0).WithMessage("نوع سوال اجباریست");
    }

    public async Task<bool> BeUniqueQuestion(string question, CancellationToken cancellationToken)
    {
        return await _context.Questions
            .AllAsync(l => l.QuestionText != question, cancellationToken);
    }
}
