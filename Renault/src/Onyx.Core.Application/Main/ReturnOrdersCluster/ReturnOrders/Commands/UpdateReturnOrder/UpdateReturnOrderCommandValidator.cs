using FluentValidation;

namespace Onyx.Application.Main.ReturnOrdersCluster.ReturnOrders.Commands.UpdateReturnOrder;
public class UpdateReturnOrderCommandValidator : AbstractValidator<UpdateReturnOrderCommand>
{
    public UpdateReturnOrderCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست"); 
        RuleFor(v => v.CustomerFirstName)
            .NotEmpty().WithMessage("نام اجباریست");
        RuleFor(v => v.CustomerLastName)
            .NotEmpty().WithMessage("نام خانوادگی اجباریست");
    }
}
