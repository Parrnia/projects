using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice.GetProduct;
public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(x => x.CustomerTypeEnum)
            .NotEmpty().WithMessage("نوع مشتری اجباریست");
    }
}