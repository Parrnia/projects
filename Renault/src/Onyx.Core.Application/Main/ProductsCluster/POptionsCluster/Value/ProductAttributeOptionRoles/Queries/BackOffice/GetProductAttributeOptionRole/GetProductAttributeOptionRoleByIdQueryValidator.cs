using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionRoles.Queries.BackOffice.GetProductAttributeOptionRole;
public class GetProductAttributeOptionRoleByIdQueryValidator : AbstractValidator<GetProductAttributeOptionRoleByIdQuery>
{
    public GetProductAttributeOptionRoleByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}