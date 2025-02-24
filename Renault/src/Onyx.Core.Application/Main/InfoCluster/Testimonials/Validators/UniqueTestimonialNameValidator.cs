using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Validators;

public record UniqueTestimonialNameValidator : IRequest<bool>
{
    public int TestimonialId { get; init; }
    public string Name { get; init; } = null!;
}

public class UniqueTestimonialNameValidatorHandler : IRequestHandler<UniqueTestimonialNameValidator, bool>
{
    private readonly IApplicationDbContext _context;

    public UniqueTestimonialNameValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UniqueTestimonialNameValidator request, CancellationToken cancellationToken)
    {
        var isUnique = await _context.Testimonials.Where(c => c.Id != request.TestimonialId)
            .AllAsync(e => e.Name != request.Name, cancellationToken);
        return isUnique;
    }
}
