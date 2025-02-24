using FluentValidation;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Queries.FrontOffice.GetTestimonialsWithPagination;
public class GetTestimonialsWithPaginationQueryValidator : AbstractValidator<GetTestimonialsWithPaginationQuery>
{
    public GetTestimonialsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}