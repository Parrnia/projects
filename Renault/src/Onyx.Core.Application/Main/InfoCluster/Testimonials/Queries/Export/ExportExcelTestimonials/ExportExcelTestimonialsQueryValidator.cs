using FluentValidation;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Queries.Export.ExportExcelTestimonials;
public class ExportExcelTestimonialsQueryValidator : AbstractValidator<ExportExcelTestimonialsQuery>
{
    public ExportExcelTestimonialsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}