using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Questions.Validators;

public record UniqueQuestionTextValidator : IRequest<bool>
{
    public int QuestionId { get; init; }
    public string QuestionText { get; init; } = null!;
}

public class UniqueQuestionTextValidatorHandler : IRequestHandler<UniqueQuestionTextValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueQuestionTextValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueQuestionTextValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Questions.Where(c => c.Id != request.QuestionId)
            .AllAsync(e => e.QuestionText != request.QuestionText, cancellationToken);
        return isUnique;
    }
}
