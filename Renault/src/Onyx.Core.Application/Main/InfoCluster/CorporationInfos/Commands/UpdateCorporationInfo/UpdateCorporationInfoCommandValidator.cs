using FluentValidation;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Helpers;

namespace Onyx.Application.Main.InfoCluster.CorporationInfos.Commands.UpdateCorporationInfo;
public class UpdateCorporationInfoCommandValidator : AbstractValidator<UpdateCorporationInfoCommand>
{
    public UpdateCorporationInfoCommandValidator(IApplicationDbContext context)
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("شناسه اجباریست");
        RuleFor(v => v.ContactUsMessage)
            .NotEmpty().WithMessage("پیام ارتباط با ما اجباریست");
        RuleFor(v => v.PoweredBy)
            .NotEmpty().WithMessage("PoweredBy اجباریست");
        RuleFor(v => v.CallUs)
            .NotEmpty().WithMessage("ارتباط با ما اجباریست");
        RuleFor(v => v.DesktopLogo)
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است")
            .NotEmpty().WithMessage("شناسه لوگوی دسکتاپ اجباریست");
        RuleFor(v => v.MobileLogo)
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است")
            .NotEmpty().WithMessage("شناسه لوگوی تلفن همراه اجباریست");
        RuleFor(v => v.FooterLogo)
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است")
            .NotEmpty().WithMessage("شناسه لوگو فوتر اجباریست");
        RuleFor(v => v.SliderBackGroundImage)
            .Must(ValidationHelperMethods.BeAValidGuid).WithMessage("فرمت شناسه ارسال شده نامعتبر است")
            .NotEmpty().WithMessage("شناسه تصویر پس زمینه اسلایدر اجباریست");
    }
}