using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Commands.DeleteTestimonial;

public class DeleteRangeTestimonialCommand : IRequest<Unit>
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteRangeTestimonialCommandHandler : IRequestHandler<DeleteRangeTestimonialCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteRangeTestimonialCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteRangeTestimonialCommand request, CancellationToken cancellationToken)
    {

        // Delete the range of IDs
        foreach (var id in request.Ids)
        {
            var entity = await _context.Testimonials
        .FindAsync(new object[] { id }, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Testimonial), id);
            }

            _context.Testimonials.Remove(entity);
            await _fileStore.RemoveStoredFile(entity.Avatar, cancellationToken);
        }


        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
