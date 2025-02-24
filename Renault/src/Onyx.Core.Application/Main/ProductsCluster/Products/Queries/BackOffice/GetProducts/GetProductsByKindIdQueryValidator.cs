using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Products.Queries.BackOffice.GetProducts;
public class GetProductsByKindIdQueryValidator : AbstractValidator<GetProductsByKindIdQuery>
{
    public GetProductsByKindIdQueryValidator()
    {
        RuleFor(x => x.KindId)
            .NotEmpty().WithMessage("شناسه نوع اجباریست");
        RuleFor(x => x.CustomerTypeEnum)
            .NotEmpty().WithMessage("نوع مشتری اجباریست");
    }
}
