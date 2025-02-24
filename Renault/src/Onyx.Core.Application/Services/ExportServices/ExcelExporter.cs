using ClosedXML.Excel;

namespace Onyx.Application.Services.ExportServices;

public class ExcelExporter
{
    public byte[] Create<T>(List<T> items,
        List<ExcelColumnInfo>? columns = null,
        string? sheetName = null, bool rightToLeftLayout = false)
    {
        var workBook = new XLWorkbook();
        var sheet = workBook.AddWorksheet(sheetName ?? "Sheet 1");
        sheet.SetRightToLeft(rightToLeftLayout);

        var properties = typeof(T).GetProperties()
            .Where(i => i.CanRead)
            .ToList();

        columns ??= properties
            .Select(prop => new ExcelColumnInfo()
            {
                Name = prop.Name,
                PropertyName = prop.Name,
                Formatter = null
            }).ToList();

        // Header
        for (var c = 0; c < columns.Count; c++)
        {
            var column = columns[c];
            sheet.Cell(1, c + 1).SetValue(column.Name);
            sheet.Cell(1, c + 1).Style.SetStyle(XLColor.White,
                XLColor.BlueGray, XLColor.White, "Segoe UI",
                12.0, true);
            if (column.ColumnWidth is not null)
                sheet.Column(c + 1).Width = column.ColumnWidth.Value;
        }

        // DataRows
        for (var r = 0; r < items.Count; r++)
        {
            var item = items[r];
            for (var c = 0; c < columns.Count; c++)
            {
                var column = columns[c];
                var property = properties.FirstOrDefault(i => i.Name == column.PropertyName);
                var valueStr = string.Empty;
                if (property is not null)
                {
                    var valueObj = property.GetValue(item);
                    if (column.Formatter is not null)
                        valueStr = column.Formatter.Invoke(valueObj);
                    else
                        valueStr = valueObj?.ToString() ?? string.Empty;
                }

                sheet.Cell(r + 2, c + 1).SetValue(valueStr);
                sheet.Cell(r + 2, c + 1).Style.SetStyle(XLColor.Gray,
                    XLColor.White, XLColor.Black, "Segoe UI",
                    11.0, false);
            }
        }

        using var ms = new MemoryStream();
        workBook.SaveAs(ms);
        return ms.ToArray();
    }
}

