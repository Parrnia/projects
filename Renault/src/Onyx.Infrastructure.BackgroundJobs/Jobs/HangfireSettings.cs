namespace Onyx.Infrastructure.BackgroundJobs.Jobs;

public class HangfireSettings
{
    public string ServerName { get; set; } = null!;
    public int WorkerCount { get; set; }
    public string DashboardUrl { get; set; } = null!;

    public string SyncDataJobName { get; set; } = null!;
    public string SyncDataSchedulerCronJob { get; set; } = null!;

    public string SyncProductPriceJobName { get; set; } = null!;
    public string SyncProductPriceSchedulerCronJob { get; set; } = null!;

    public string SendFailedRequestsJobName { get; set; } = null!;
    public string SendFailedRequestsSchedulerCronJob { get; set; } = null!;

    public string RemoveExpiredLoggedRequestsJobName { get; set; } = null!;
    public string RemoveExpiredLoggedRequestsSchedulerCronJob { get; set; } = null!;

    public string RemoveExpiredTempImageFilesJobName { get; set; } = null!;
    public string RemoveExpiredTempImageFilesSchedulerCronJob { get; set; } = null!;

    public string VerifyWaitingPaymentsJobName { get; set; } = null!;
    public string VerifyWaitingPaymentsSchedulerCronJob { get; set; } = null!;

}

