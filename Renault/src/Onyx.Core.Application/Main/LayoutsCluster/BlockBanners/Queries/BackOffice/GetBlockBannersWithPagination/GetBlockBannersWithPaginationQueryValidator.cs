using FluentValidation;

namespace Onyx.Application.Main.LayoutsCluster.BlockBanners.Queries.BackOffice.GetBlockBannersWithPagination;
public class GetBlockBannersWithPaginationQueryValidator : AbstractValidator<GetBlockBannersWithPaginationQuery>
{
    public GetBlockBannersWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}