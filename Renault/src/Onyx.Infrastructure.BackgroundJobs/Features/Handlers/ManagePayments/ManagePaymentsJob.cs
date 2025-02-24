using Hangfire;

namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.ManagePayments;

public class ManagePaymentsJob
{
    private readonly ManagePaymentsHandler _managePaymentsHandler;

    public ManagePaymentsJob(ManagePaymentsHandler managePaymentsHandler)
    {
        _managePaymentsHandler = managePaymentsHandler;
    }

    [DisableConcurrentExecution(1)]
    [AutomaticRetry(Attempts = 1, DelaysInSeconds = new int[] { 900 }, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public Task StartVerifyWaitingPayments()
    {
        return _managePaymentsHandler.VerifyWaitingPayments();
    }
}