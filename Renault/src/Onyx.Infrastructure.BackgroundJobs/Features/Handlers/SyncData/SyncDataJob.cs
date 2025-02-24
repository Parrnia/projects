using Hangfire;

namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData;

public class SyncDataJob
{
    private readonly DataMigrationHandler _dataMigrationHandler;

    public SyncDataJob(DataMigrationHandler dataMigrationHandler)
    {
        _dataMigrationHandler = dataMigrationHandler;
    }

    //[DisableConcurrentExecution(1)]
    [AutomaticRetry(Attempts = 5, DelaysInSeconds = new int[] { 1800 }, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public Task StartSyncDataFrom7Soft()
    {
        return _dataMigrationHandler.MapData();
    }

    [AutomaticRetry(Attempts = 5, DelaysInSeconds = new int[] { 1800 }, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public Task StartSyncProductPriceAndCountFrom7Soft()
    {
        return _dataMigrationHandler.SyncProductPriceAndCount();
    }
}