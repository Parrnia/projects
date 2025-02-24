using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Queries.FrontOffice.GetTestimonial;

public record GetTestimonialByIdQuery(int Id) : IRequest<TestimonialByIdDto?>;

public class GetTestimonialByIdQueryHandler : IRequestHandler<GetTestimonialByIdQuery, TestimonialByIdDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTestimonialByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TestimonialByIdDto?> Handle(GetTestimonialByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Testimonials
            .ProjectTo<TestimonialByIdDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
    }
}
