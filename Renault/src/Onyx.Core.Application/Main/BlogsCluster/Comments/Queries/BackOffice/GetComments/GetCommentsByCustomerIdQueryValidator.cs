using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.BlogsCluster.Comments.Queries.BackOffice.GetComments;
public class GetCommentsByCustomerIdQueryValidator : AbstractValidator<GetCommentsByCustomerIdQuery>
{
    public GetCommentsByCustomerIdQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}