namespace Onyx.Domain.Entities.InfoCluster;
public class Question : BaseAuditableEntity
{
    /// <summary>
    /// متن سوال
    /// </summary>
    public string QuestionText { get; set; } = null!;
    /// <summary>
    /// متن پاسخ
    /// </summary>
    public string AnswerText { get; set; } = null!;
    /// <summary>
    /// موضوع سوال
    /// </summary>
    public QuestionType QuestionType { get; set; }
}
