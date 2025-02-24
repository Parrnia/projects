using Hangfire;

namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.ManageFiles;

public class ManageFilesJob
{
    private readonly ManageFilesHandler _manageFilesHandler;

    public ManageFilesJob(ManageFilesHandler manageFilesHandler)
    {
        _manageFilesHandler = manageFilesHandler;
    }

    [DisableConcurrentExecution(1)]
    [AutomaticRetry(Attempts = 1, DelaysInSeconds = new int[] { 900 }, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public Task StartRemoveExpiredTempImageFiles()
    {
        return _manageFilesHandler.RemoveExpiredTempImageFiles();
    }
}