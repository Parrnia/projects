using System.ComponentModel;

namespace Onyx.Domain.Enums;

/// <summary>
/// نوع هویت
/// </summary>
public enum PersonType
{
    [Description("حقیقی")]
    Natural = 1,

    [Description("حقوقی")]
    Legal = 2,

}