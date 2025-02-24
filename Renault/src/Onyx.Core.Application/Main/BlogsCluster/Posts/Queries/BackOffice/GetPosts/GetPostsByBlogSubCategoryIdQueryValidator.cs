using FluentValidation;

namespace Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice.GetPosts;
public class GetPostsByBlogSubCategoryIdQueryValidator : AbstractValidator<GetPostsByBlogSubCategoryIdQuery>
{
    public GetPostsByBlogSubCategoryIdQueryValidator()
    {
        RuleFor(x => x.BlogSubCategoryId)
            .NotEmpty().WithMessage("شناسه دسته بندی بلاگ اجباریست");
    }
}