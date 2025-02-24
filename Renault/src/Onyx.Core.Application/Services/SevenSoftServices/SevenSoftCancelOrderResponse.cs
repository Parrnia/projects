using System.Text.Json.Serialization;

namespace Onyx.Application.Services.SevenSoftServices;

public class SevenSoftCancelOrderResponse
{
    /// <summary>
    /// وضعیت درخواست
    /// </summary>
    [JsonPropertyName("<IsValid>k__BackingField")]
    public bool IsValid { get; set; }
    /// <summary>
    /// پیغام
    /// </summary>
    [JsonPropertyName("<Message>k__BackingField")]
    public string? Message { get; set; }
    /// <summary>
    /// مدل
    /// </summary>
    [JsonPropertyName("<Model>k__BackingField")]
    public object? Model { get; set; }
}