
using ClosedXML.Excel;

namespace Onyx.Application.Services.ExportServices;

public static class ExcelExtensions
{
    public static IXLStyle SetStyle(this IXLStyle style,
        XLColor? borderColor = null, XLColor? backgroundColor = null,
        XLColor? fontColor = null, string? fontName = null,
        double? fontSize = null, bool? isBold = null)
    {
        if (borderColor is not null)
        {
            style
                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                .Border.SetLeftBorderColor(borderColor)
                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                .Border.SetTopBorderColor(borderColor)
                .Border.SetRightBorder(XLBorderStyleValues.Thin)
                .Border.SetRightBorderColor(borderColor)
                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                .Border.SetBottomBorderColor(borderColor);
        }

        if (backgroundColor is not null)
        {
            style.Fill.SetBackgroundColor(backgroundColor);
        }

        if (fontColor is not null)
        {
            style.Font.SetFontColor(fontColor);
        }

        if (fontName is not null)
        {
            style.Font.SetFontName(fontName);
        }

        if (fontSize is not null)
        {
            style.Font.SetFontSize(fontSize.Value);
        }

        if (isBold is not null)
        {
            style.Font.SetBold(isBold.Value);
        }

        return style;
    }
}