using FluentValidation;

namespace Onyx.Application.Main.InfoCluster.Testimonials.Queries.FrontOffice.GetTestimonial;
public class GetTestimonialByIdQueryValidator : AbstractValidator<GetTestimonialByIdQuery>
{
    public GetTestimonialByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
    }
}