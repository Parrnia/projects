using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.ProductStatuses.Queries.BackOffice.GetProductStatus;
public class GetProductStatusByIdQueryValidator : AbstractValidator<GetProductStatusByIdQuery>
{
    public GetProductStatusByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}