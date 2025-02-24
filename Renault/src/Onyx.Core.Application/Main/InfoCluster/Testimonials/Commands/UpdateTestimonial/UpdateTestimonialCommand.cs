using MediatR;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Commands.UpdateTestimonial;
public record UpdateTestimonialCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string Position { get; init; } = null!;
    public Guid Avatar { get; init; }
    public int Rating { get; init; }
    public string Review { get; init; } = null!;
    public bool IsActive { get; init; }
    public int AboutUsId { get; init; }
}

public class UpdateTestimonialCommandHandler : IRequestHandler<UpdateTestimonialCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public UpdateTestimonialCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<Unit> Handle(UpdateTestimonialCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Testimonials
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Testimonial), request.Id);
        }

        entity.Name = request.Name;
        entity.Position = request.Position;
        entity.Rating = request.Rating;
        entity.Review = request.Review;
        entity.AboutUsId = request.AboutUsId;
        entity.IsActive = request.IsActive;

        if (entity.Avatar != request.Avatar)
        {
            await _fileStore.RemoveStoredFile(entity.Avatar, cancellationToken);

            await _fileStore.SaveStoredFile(
                request.Avatar,
                FileCategory.ProductBrand.ToString(),
                FileCategory.ProductBrand,
                null,
                false,
                cancellationToken);
            entity.Avatar = request.Avatar;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}