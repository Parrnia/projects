using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Kinds.Queries.FrontOffice.GetKindsWithPagination.GetKindsWithPagination;
public class GetKindsWithPaginationQueryValidator : AbstractValidator<GetKindsWithPaginationQuery>
{
    public GetKindsWithPaginationQueryValidator()
    {
        RuleFor(x => x.ModelId)
            .NotEmpty().WithMessage("شناسه مدل اجباریست");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}
