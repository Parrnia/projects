using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice.GetProducts;
public class GetAllProductsQueryValidator : AbstractValidator<GetAllProductsQuery>
{
    public GetAllProductsQueryValidator()
    {
        RuleFor(x => x.CustomerTypeEnum)
            .NotEmpty().WithMessage("نوع مشتری اجباریست");
    }
}
