using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.BackOffice.GetFamiliesWithPagination;
public class GetFamiliesWithPaginationQueryValidator : AbstractValidator<GetFamiliesWithPaginationQuery>
{
    public GetFamiliesWithPaginationQueryValidator()
    {
        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("شناسه برند اجباریست");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}