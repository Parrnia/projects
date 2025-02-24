using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Onyx.Application.Common.Interfaces;

namespace Onyx.Application.Main.InfoCluster.Questions.Queries.FrontOffice.GetQuestions;
public record GetAllQuestionsQuery : IRequest<List<AllQuestionDto>>;

public class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, List<AllQuestionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllQuestionsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AllQuestionDto>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Questions
            .Where(c => c.IsActive)
            .OrderBy(x => x.QuestionType)
            .ProjectTo<AllQuestionDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
