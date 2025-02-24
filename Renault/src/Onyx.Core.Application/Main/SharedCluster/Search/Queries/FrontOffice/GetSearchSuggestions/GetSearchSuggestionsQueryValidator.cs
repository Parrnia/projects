using FluentValidation;

namespace Onyx.Application.Main.SharedCluster.Search.Queries.FrontOffice.GetSearchSuggestions;
public class GetSearchSuggestionsQueryValidator : AbstractValidator<GetSearchSuggestionsQuery>
{
    public GetSearchSuggestionsQueryValidator()
    {
        RuleFor(x => x.CustomerTypeEnum)
            .NotEmpty().WithMessage("نوع مشتری اجباریست");
        RuleFor(x => x.ProductLimit)
            .GreaterThanOrEqualTo(1).WithMessage("محدوده محصول باید بزرگتر یا مساوی یک باشد");
        RuleFor(x => x.ProductCategoryLimit)
            .GreaterThanOrEqualTo(1).WithMessage("محدوده دسته بندی محصول باید بزرگتر یا مساوی یک باشد");
    }
}