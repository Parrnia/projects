using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.BlogsCluster.Posts.Queries.BackOffice.GetPosts;
public class GetPostsByUserIdQueryValidator : AbstractValidator<GetPostsByUserIdQuery>
{
    public GetPostsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("شناسه کاربر اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}