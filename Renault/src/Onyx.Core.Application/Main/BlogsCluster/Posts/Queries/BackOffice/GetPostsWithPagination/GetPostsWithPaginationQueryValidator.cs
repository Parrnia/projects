using FluentValidation;

namespace Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice.GetPostsWithPagination;
public class GetPostsWithPaginationQueryValidator : AbstractValidator<GetPostsWithPaginationQuery>
{
    public GetPostsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("شماره صفحه باید بزرگتر یا مساوی یک باشد");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("تعداد آیتم های صفحه باید بزرگتر یا مساوی یک باشد");
    }
}