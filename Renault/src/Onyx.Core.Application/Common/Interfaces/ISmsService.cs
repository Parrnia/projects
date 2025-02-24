namespace Onyx.Application.Common.Interfaces;
public interface ISmsService
{
    public Task SendSms(string phoneNumber, string message);
}
