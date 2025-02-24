using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Questions.Commands.UpdateQuestion;
public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    private readonly IApplicationDbContext _context;
    private int _id;
    public UpdateQuestionCommandValidator(IApplicationDbContext context)
    {
        _context = context;


        RuleFor(v => v.Id)
            .MustAsync(GetIdForUniqueness).NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.QuestionText)
            .MustAsync(BeUniqueQuestion).WithMessage("سوالی با این متن موجود است")
            .NotEmpty().WithMessage("متن سوال اجباریستت");
        RuleFor(v => v.AnswerText)
            .NotEmpty().WithMessage("متن پاسخ اجباریست");
        RuleFor(v => v.QuestionType)
            .Must(c => c >= 0).WithMessage("نوع سوال اجباریست");
    }

    public async Task<bool> BeUniqueQuestion(string question, CancellationToken cancellationToken)
    {
        return await _context.Questions
            .Where(l => l.Id != _id)
            .AllAsync(l => l.QuestionText != question, cancellationToken);
    }
    public Task<bool> GetIdForUniqueness(int requestId, CancellationToken cancellationToken)
    {
        this._id = requestId;
        return Task.FromResult(true);
    }
}