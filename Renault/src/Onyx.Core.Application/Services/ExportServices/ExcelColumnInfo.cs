namespace Onyx.Application.Services.ExportServices;

public class ExcelColumnInfo
{
    public string Name { get; set; }
    public string PropertyName { get; set; }
    public double? ColumnWidth { get; set; }
    public Func<object?, string?>? Formatter { get; set; }
}