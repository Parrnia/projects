using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.InfoCluster.Questions.Commands.CreateQuestion;
public record CreateQuestionCommand : IRequest<int>
{
    public string QuestionText { get; init; } = null!;
    public string AnswerText { get; init; } = null!;
    public int QuestionType { get; init; }
    public bool IsActive { get; init; }
}

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateQuestionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var entity = new Question()
        {
            QuestionText = request.QuestionText,
            AnswerText = request.AnswerText,
            QuestionType = (QuestionType) request.QuestionType,
            IsActive = request.IsActive
        };


        _context.Questions.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
