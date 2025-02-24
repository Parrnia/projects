using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Commands.DeleteTestimonial;

public record DeleteTestimonialCommand(int Id) : IRequest<Unit>;

public class DeleteTestimonialCommandHandler : IRequestHandler<DeleteTestimonialCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public DeleteTestimonialCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(DeleteTestimonialCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Testimonials
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Testimonial), request.Id);
        }

        _context.Testimonials.Remove(entity);
        await _fileStore.RemoveStoredFile(entity.Avatar, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}