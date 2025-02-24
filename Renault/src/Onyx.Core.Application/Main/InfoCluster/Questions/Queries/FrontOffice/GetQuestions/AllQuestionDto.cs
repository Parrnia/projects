using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.InfoCluster.Questions.Queries.FrontOffice.GetQuestions;
public class AllQuestionDto : IMapFrom<Question>
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = null!;
    public string AnswerText { get; set; } = null!;
    public QuestionType QuestionType { get; set; }
}
