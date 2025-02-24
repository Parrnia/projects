using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.InfoCluster;
using Onyx.Domain.Enums;

namespace Onyx.Application.Main.InfoCluster.Questions.Queries.FrontOffice.GetQuestion;
public class QuestionByIdDto : IMapFrom<Question>
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = null!;
    public string AnswerText { get; set; } = null!;
    public QuestionType QuestionType { get; set; }
}
