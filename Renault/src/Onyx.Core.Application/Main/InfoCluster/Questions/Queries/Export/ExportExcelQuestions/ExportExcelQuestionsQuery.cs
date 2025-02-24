using System.Linq.Expressions;
using MediatR;
using Onyx.Application.Common.Interfaces;
using Onyx.Domain.Entities.InfoCluster;

namespace Onyx.Application.Main.InfoCluster.Questions.Queries.Export.ExportExcelQuestions;

public record ExportExcelQuestionsQuery() : IRequest<byte[]>
{
    public string? SearchText { get; set; }
    public int? PageNumber { get; init; } = 1;
    public int? PageSize { get; init; }
    public DateTime? StartCreationDate { get; set; }
    public DateTime? EndCreationDate { get; set; }
    public DateTime? StartChangeDate { get; set; }
    public DateTime? EndChangeDate { get; set; }
}

public class ExportExcelQuestionsQueryHandler : IRequestHandler<ExportExcelQuestionsQuery, byte[]>
{
    private readonly IApplicationDbContext _context;
    private readonly IExportServices _exportServices;


    public ExportExcelQuestionsQueryHandler(IApplicationDbContext context, IExportServices exportServices)
    {
        _context = context;
        _exportServices = exportServices;
    }

    public async Task<byte[]> Handle(ExportExcelQuestionsQuery request, CancellationToken cancellationToken)
    {
        var questions = _context.Questions
            .OrderBy(x => x.QuestionType);
        
        var selectedProperties = new List<Expression<Func<Question, object>>?>()
        {
            c => c.QuestionText,
            c => c.AnswerText,
            c => c.QuestionType,
            c => c.IsActive
        };

        var exported = await _exportServices.Export(
            questions,
            selectedProperties,
            request.SearchText,
            request.PageNumber,
            request.PageSize,
            request.StartCreationDate,
            request.EndCreationDate,
            request.StartChangeDate,
            request.EndChangeDate,
            cancellationToken);

        var exportedExcel = _exportServices.ExportExcel(exported, selectedProperties);
        return exportedExcel;
    }
}
