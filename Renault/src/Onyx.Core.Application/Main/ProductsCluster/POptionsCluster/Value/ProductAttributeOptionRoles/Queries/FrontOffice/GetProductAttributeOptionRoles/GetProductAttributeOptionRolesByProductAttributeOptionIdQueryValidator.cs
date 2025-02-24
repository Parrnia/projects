using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.FrontOffice.GetProductAttributeOptionRoles;
public class GetProductAttributeOptionRolesByProductAttributeOptionIdQueryValidator : AbstractValidator<GetProductAttributeOptionRolesByProductAttributeOptionIdQuery>
{
    public GetProductAttributeOptionRolesByProductAttributeOptionIdQueryValidator()
    {
        RuleFor(x => x.ProductAttributeOptionId)
            .NotEmpty().WithMessage("شناسه نوع آپشن محصول اجباریست");
    }
}
