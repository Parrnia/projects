using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Onyx.Application.Common.Interfaces;
using Onyx.Application.Settings;

namespace Onyx.Application.Services;
public class SmsService : ISmsService
{
    private readonly ApplicationSettings _applicationSettings;
    private readonly HttpClient _client;

    public SmsService(IOptions<ApplicationSettings> applicationSettings)
    {
        _applicationSettings = applicationSettings.Value;
        _client = new HttpClient();
    }

    public async Task SendSms(string phoneNumber, string message)
    {
        var orderSmsDto = new OrderSmsCommand
        {
            MobileNumber = phoneNumber,
            SmsText = message + "\n" + "لغو:11"
        };

        string jsonBody = JsonSerializer.Serialize(orderSmsDto);

        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        try
        {
            var response = await _client.PostAsync(_applicationSettings.SendSmsUrl, content);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}