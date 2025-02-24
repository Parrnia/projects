using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Queries.BackOffice.GetTestimonial;

public record GetTestimonialByIdQuery(int Id) : IRequest<TestimonialDto?>;

public class GetTestimonialByIdQueryHandler : IRequestHandler<GetTestimonialByIdQuery, TestimonialDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTestimonialByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TestimonialDto?> Handle(GetTestimonialByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Testimonials
            .ProjectTo<TestimonialDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
