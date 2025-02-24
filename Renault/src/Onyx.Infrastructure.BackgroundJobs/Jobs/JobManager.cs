using Hangfire;
using Onyx.Infrastructure.BackgroundJobs.Features.Handlers.ManageFiles;
using Onyx.Infrastructure.BackgroundJobs.Features.Handlers.ManagePayments;
using Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SendRequest;
using Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData;

namespace Onyx.Infrastructure.BackgroundJobs.Jobs;

public class JobManager
{
    public static void Initialize(HangfireSettings settings)
    {
        RecurringJob.AddOrUpdate<SendRequestJob>(settings.SendFailedRequestsJobName,
            job => job.StartSendFailedRequests(),
            settings.SendFailedRequestsSchedulerCronJob);

        RecurringJob.AddOrUpdate<SendRequestJob>(settings.RemoveExpiredLoggedRequestsJobName,
            job => job.StartRemoveExpiredLoggedRequests(),
            settings.RemoveExpiredLoggedRequestsSchedulerCronJob);


        RecurringJob.AddOrUpdate<SyncDataJob>(settings.SyncDataJobName,
            job => job.StartSyncDataFrom7Soft(),
            settings.SyncDataSchedulerCronJob);

        RecurringJob.AddOrUpdate<SyncDataJob>(settings.SyncProductPriceJobName,
            job => job.StartSyncProductPriceAndCountFrom7Soft(),
            settings.SyncProductPriceSchedulerCronJob);

        RecurringJob.AddOrUpdate<ManageFilesJob>(settings.RemoveExpiredTempImageFilesJobName,
            job => job.StartRemoveExpiredTempImageFiles(),
            settings.RemoveExpiredTempImageFilesSchedulerCronJob);

        RecurringJob.AddOrUpdate<ManagePaymentsJob>(settings.VerifyWaitingPaymentsJobName,
            job => job.StartVerifyWaitingPayments(),
            settings.VerifyWaitingPaymentsSchedulerCronJob);
    }
}
