using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Queries.FrontOffice.GetProductAttributeOptions;
public class GetAllProductAttributeOptionsByProductIdQueryValidator : AbstractValidator<GetAllProductAttributeOptionsByProductIdQuery>
{
    public GetAllProductAttributeOptionsByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
    }
}