using System.Diagnostics.CodeAnalysis;

namespace RPWebSvrCli.Config;

[ExcludeFromCodeCoverage]
public class Logging
{
    public LogLevel LogLevel { get; set; } = new();
}
