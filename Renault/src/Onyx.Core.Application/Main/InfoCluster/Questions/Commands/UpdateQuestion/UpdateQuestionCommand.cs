using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.InfoCluster.Questions.Commands.UpdateQuestion;
public record UpdateQuestionCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string QuestionText { get; init; } = null!;
    public string AnswerText { get; init; } = null!;
    public int QuestionType { get; init; }
    public bool IsActive { get; init; }
}

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdateQuestionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Questions
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Question), request.Id);
        }

        entity.QuestionText = request.QuestionText;
        entity.AnswerText = request.AnswerText;
        entity.QuestionType = (QuestionType) request.QuestionType;
        entity.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}