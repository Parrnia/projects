using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.ProductImages.Queries.FrontOffice.GetProductImages;
public class GetProductImagesByProductIdQueryValidator : AbstractValidator<GetProductImagesByProductIdQuery>
{
    public GetProductImagesByProductIdQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("شناسه محصول اجباریست");
    }
}