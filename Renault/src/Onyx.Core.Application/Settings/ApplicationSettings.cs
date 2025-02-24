namespace Onyx.Application.Settings;

public class ApplicationSettings
{
    public string SevenSoftBaseUrl { get; set; } = null!;
    public string SevenSoftBaseUrlSecond { get; set; } = null!;
    public string SevenSoftUserName { get; set; } = null!;
    public string SevenSoftPassword { get; set; } = null!;
    public string SendSmsUrl { get; set; } = null!;
    public string PersistFolder { get; set; } = null!;
    public string UploadTempFolder { get; set; } = null!;
    public double TaxPercent { get; set; }
}
