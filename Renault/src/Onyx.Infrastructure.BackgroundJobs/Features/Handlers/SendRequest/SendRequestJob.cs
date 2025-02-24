using Hangfire;

namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SendRequest;

public class SendRequestJob
{
    private readonly SendRequestHandler _sendRequestHandler;

    public SendRequestJob(SendRequestHandler sendRequestHandler)
    {
        _sendRequestHandler = sendRequestHandler;
    }

    [DisableConcurrentExecution(1)]
    [AutomaticRetry(Attempts = 1, DelaysInSeconds = new int[] { 900 }, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public Task StartSendFailedRequests()
    {
        return _sendRequestHandler.SendFailedCancelOrderRequests();
    }

    [DisableConcurrentExecution(1)]
    [AutomaticRetry(Attempts = 5, DelaysInSeconds = new int[] { 1800 }, LogEvents = true, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
    public Task StartRemoveExpiredLoggedRequests()
    {
        return _sendRequestHandler.RemoveExpiredLoggedRequests();
    }
}