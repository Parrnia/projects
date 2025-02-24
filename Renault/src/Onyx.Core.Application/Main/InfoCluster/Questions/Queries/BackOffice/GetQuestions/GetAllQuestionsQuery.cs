using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Questions.Queries.BackOffice.GetQuestions;
public record GetAllQuestionsQuery : IRequest<List<QuestionDto>>;

public class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, List<QuestionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllQuestionsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<QuestionDto>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Questions.AsNoTracking()
            .OrderBy(x => x.QuestionType)
            .ProjectTo<QuestionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
