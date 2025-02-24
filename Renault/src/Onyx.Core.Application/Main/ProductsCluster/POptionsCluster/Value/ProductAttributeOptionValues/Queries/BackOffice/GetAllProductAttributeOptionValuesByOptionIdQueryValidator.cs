using FluentValidation;
using Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionValues.Queries.FrontOffice;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionValues.Queries.BackOffice;
public class GetAllProductAttributeOptionValuesByOptionIdQueryValidator : AbstractValidator<GetAllProductAttributeOptionValuesByOptionIdQuery>
{
    public GetAllProductAttributeOptionValuesByOptionIdQueryValidator()
    {
        RuleFor(x => x.OptionId)
            .NotEmpty().WithMessage("شناسه آپشن اجباریست");
    }
}