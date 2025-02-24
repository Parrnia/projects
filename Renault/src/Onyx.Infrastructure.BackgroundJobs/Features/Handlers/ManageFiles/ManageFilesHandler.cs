using Microsoft.Extensions.Options;
using Onyx.Application.Settings;

namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.ManageFiles;
public class ManageFilesHandler
{
    private readonly ApplicationSettings _applicationSettings;

    public ManageFilesHandler(
        IOptions<ApplicationSettings> applicationSettings)
    {
        _applicationSettings = applicationSettings.Value;
    }

    public Task<bool> RemoveExpiredTempImageFiles()
    {
        var folderPath = _applicationSettings.UploadTempFolder;
        var oneWeek = TimeSpan.FromDays(7);
        var currentTime = DateTime.Now;

        try
        {
            var files = Directory.GetFiles(folderPath);

            foreach (var file in files)
            {
                var creationTime = File.GetCreationTime(file);

                if (currentTime - creationTime > oneWeek)
                {
                    File.Delete(file);
                }
            }

            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            return Task.FromResult(false);
        }
    }
}