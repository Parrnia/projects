using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.POptionsCluster.Value.ProductAttributeOptions.Queries.BackOffice;
public class GetAllProductAttributeOptionValueByOptionIdQueryValidator : AbstractValidator<GetAllProductAttributeOptionValueByOptionIdQuery>
{
    public GetAllProductAttributeOptionValueByOptionIdQueryValidator()
    {
        RuleFor(x => x.OptionId)
            .NotEmpty().WithMessage("شناسه آپشن اجباریست");
    }
}