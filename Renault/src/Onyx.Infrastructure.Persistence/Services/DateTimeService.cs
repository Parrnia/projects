using Onyx.Application.Common.Interfaces;

namespace Onyx.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
