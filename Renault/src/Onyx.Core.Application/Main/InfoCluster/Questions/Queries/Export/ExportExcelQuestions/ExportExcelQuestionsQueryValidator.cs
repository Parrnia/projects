using FluentValidation;

namespace Onyx.Application.Main.InfoCluster.Questions.Queries.Export.ExportExcelQuestions;
public class ExportExcelQuestionsQueryValidator : AbstractValidator<ExportExcelQuestionsQuery>
{
    public ExportExcelQuestionsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}