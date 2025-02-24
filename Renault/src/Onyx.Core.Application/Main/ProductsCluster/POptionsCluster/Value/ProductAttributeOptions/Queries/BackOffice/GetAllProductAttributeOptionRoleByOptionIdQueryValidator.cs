using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Queries.BackOffice;
public class GetAllProductAttributeOptionRoleByOptionIdQueryValidator : AbstractValidator<GetAllProductAttributeOptionRoleByOptionIdQuery>
{
    public GetAllProductAttributeOptionRoleByOptionIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه آپشن اجباریست");
    }
}