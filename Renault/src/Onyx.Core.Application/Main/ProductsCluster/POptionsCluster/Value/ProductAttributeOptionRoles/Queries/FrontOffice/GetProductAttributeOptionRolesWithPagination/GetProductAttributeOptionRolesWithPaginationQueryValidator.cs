using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.FrontOffice.GetProductAttributeOptionRolesWithPagination;
public class GetProductAttributeOptionRolesWithPaginationQueryValidator : AbstractValidator<GetProductAttributeOptionRolesWithPaginationQuery>
{
    public GetProductAttributeOptionRolesWithPaginationQueryValidator()
    {
        RuleFor(x => x.ProductAttributeOptionId)
            .NotEmpty().WithMessage("شناسه نوع آپشن محصول اجباریست");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}
