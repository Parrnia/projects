using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Common.Mappings;
using Onyx.Application.Common.Models;

namespace Onyx.Application.Main.InfoCluster.Questions.Queries.FrontOffice.GetQuestionsWithPagination;
public record GetQuestionsWithPaginationQuery : IRequest<PaginatedList<QuestionWithPaginationDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetQuestionsWithPaginationQueryHandler : IRequestHandler<GetQuestionsWithPaginationQuery, PaginatedList<QuestionWithPaginationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetQuestionsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<QuestionWithPaginationDto>> Handle(GetQuestionsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Questions
            .Where(c => c.IsActive)
            .OrderBy(x => x.QuestionType)
            .ProjectTo<QuestionWithPaginationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
