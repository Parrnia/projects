using FluentValidation;

namespace Onyx.Application.Main.SharedCluster.Search.Queries.FrontOffice.GetFilterResults;
public class GetFilterResultsQueryValidator : AbstractValidator<GetFilterResultsQuery>
{
    public GetFilterResultsQueryValidator()
    {
        RuleFor(x => x.CustomerType)
            .IsInEnum().WithMessage("نوع مشتری اجباریست");
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");
        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(1).WithMessage("محدوده باید بزرگتر یا مساوی یک باشد");
        RuleFor(x => x.Sort)
            .NotEmpty().WithMessage("مرتب سازی اجباریست");
    }
}
