using FluentValidation;

namespace Onyx.Application.Main.BrandsCluster.Models.Queries.FrontOffice.GetModelsWithPagination.GetModelsWithPagination;
public class GetModelsWithPaginationQueryValidator : AbstractValidator<GetModelsWithPaginationQuery>
{
    public GetModelsWithPaginationQueryValidator()
    {
        RuleFor(x => x.FamilyId)
            .NotEmpty().WithMessage("شناسه خانواده اجباریست");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}
