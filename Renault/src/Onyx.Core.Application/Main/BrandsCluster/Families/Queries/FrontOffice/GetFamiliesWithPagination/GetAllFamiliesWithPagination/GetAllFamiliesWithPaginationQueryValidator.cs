using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Families.Queries.FrontOffice.GetFamiliesWithPagination.GetAllFamiliesWithPagination;
public class GetAllFamiliesWithPaginationQueryValidator : AbstractValidator<GetAllFamiliesWithPaginationQuery>
{
    public GetAllFamiliesWithPaginationQueryValidator()
    {
        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("شناسه برند اجباریست");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}