using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.Questions.Commands.DeleteQuestion;

public class DeleteRangeQuestionCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeQuestionCommandHandler : IRequestHandler<DeleteRangeQuestionCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteRangeQuestionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteRangeQuestionCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.Questions
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Question), id);
            }

            _context.Questions.Remove(entity);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
