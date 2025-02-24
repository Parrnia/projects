using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Commands.CreateTestimonial;
public record CreateTestimonialCommand : IRequest<int>
{
    public string Name { get; init; } = null!;
    public string Position { get; init; } = null!;
    public Guid Avatar { get; init; }
    public int Rating { get; init; }
    public string Review { get; init; } = null!;
    public bool IsActive { get; init; }
    public int AboutUsId { get; init; }
}

public class CreateTestimonialCommandHandler : IRequestHandler<CreateTestimonialCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStore _fileStore;

    public CreateTestimonialCommandHandler(IApplicationDbContext context, IFileStore fileStore)
    {
        _context = context;
        _fileStore = fileStore;
    }

    public async Task<int> Handle(CreateTestimonialCommand request, CancellationToken cancellationToken)
    {
        var entity = new Testimonial()
        {
            Name = request.Name,
            Position = request.Position,
            Avatar = request.Avatar,
            Rating = request.Rating,
            Review = request.Review,
            IsActive = request.IsActive,
            AboutUsId = request.AboutUsId
        };

        await _fileStore.SaveStoredFile(
            request.Avatar,
            FileCategory.Testimonial.ToString(),
            FileCategory.Testimonial,
            null,
            false,
            cancellationToken);


        _context.Testimonials.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
