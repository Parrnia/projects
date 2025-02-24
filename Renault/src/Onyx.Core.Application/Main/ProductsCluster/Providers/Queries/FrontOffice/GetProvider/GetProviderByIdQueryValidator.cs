using FluentValidation;

namespace Onyx.Application.Main.ProductsCluster.Providers.Queries.FrontOffice.GetProvider;
public class GetProviderByIdQueryValidator : AbstractValidator<GetProviderByIdQuery>
{
    public GetProviderByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}