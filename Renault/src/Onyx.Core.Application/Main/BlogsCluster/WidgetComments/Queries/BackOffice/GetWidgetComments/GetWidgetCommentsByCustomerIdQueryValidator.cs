using FluentValidation;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.BlogsCluster.WidgetComments.Queries.BackOffice.GetWidgetComments;
public class GetWidgetCommentsByCustomerIdQueryValidator : AbstractValidator<GetWidgetCommentsByCustomerIdQuery>
{
    public GetWidgetCommentsByCustomerIdQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("شناسه مشتری اجباریست")
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است");
    }
}