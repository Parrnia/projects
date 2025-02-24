using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptionValues.Queries.FrontOffice;
public class GetAllProductAttributeOptionValuesByOptionIdQueryValidator : AbstractValidator<GetAllProductAttributeOptionValuesByOptionIdQuery>
{
    public GetAllProductAttributeOptionValuesByOptionIdQueryValidator()
    {
        RuleFor(x => x.OptionId)
            .NotEmpty().WithMessage("شناسه آپشن اجباریست");
    }
}