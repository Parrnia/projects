using Microsoft.Extensions.Options;
using Onyx.Application.Settings;
using Onyx.Domain.Entities.RequestsCluster;
using Onyx.Domain.Enums;
using Onyx.Infrastructure.Persistence;

namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers;
public class SharedService
{
    private readonly ApplicationDbContext _context;
    private readonly ApplicationSettings _applicationSettings;

    public SharedService(
        ApplicationDbContext context,
        IOptions<ApplicationSettings> applicationSettings)
    {
        _context = context;
        _applicationSettings = applicationSettings.Value;
    }

    public async Task<string> Authenticate()
    {
        var accessToken = "";


        using var client = new HttpClient();
        client.BaseAddress = new Uri(_applicationSettings.SevenSoftBaseUrl);

        var authenticate = new Authenticate()
        {
            UserName = _applicationSettings.SevenSoftUserName,
            Password = _applicationSettings.SevenSoftPassword
        };

        var response = client.PostAsJsonAsync("Login/Authenticate/", authenticate).Result;
        if (response.IsSuccessStatusCode)
        {
            string resContent = await response.Content.ReadAsStringAsync();
            accessToken = resContent;
        }

        var requestLog = new RequestLog
        {
            ApiAddress = client.BaseAddress + "Login/Authenticate/",
            ResponseStatus = response.StatusCode,
            RequestType = RequestType.Authenticate,
            Created = DateTime.Now,
            ApiType = ApiType.Post
        };
        await _context.RequestLogs.AddAsync(requestLog);
        await _context.SaveChangesAsync();

        return accessToken;
    }
}