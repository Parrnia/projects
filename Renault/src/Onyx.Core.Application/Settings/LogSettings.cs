using Microsoft.Extensions.Logging;

namespace Onyx.Application.Settings;

public class LogSettings
{
    public string ServiceName { get; set; }
    public string LogLevel { get; set; }
    public Dictionary<string, LogLevel>? Overrides { get; set; }

    public SeqLogSettings? Seq { get; set; }
    public FileLogSettings? File { get; set; }
    public ConsoleLogSettings? Console { get; set; }
}

public class SeqLogSettings
{
    public bool Enabled { get; set; }
    public string ServerUrl { get; set; }
    public string ApiKey { get; set; }
}

public class FileLogSettings
{
    public bool Enabled { get; set; }
    public string Path { get; set; }
    public string RollingInterval { get; set; }
    public int? FilesCountLimit { get; set; }
    public long? FilesSizeLimit { get; set; }
}

public class ConsoleLogSettings
{
    public bool Enabled { get; set; }
}

