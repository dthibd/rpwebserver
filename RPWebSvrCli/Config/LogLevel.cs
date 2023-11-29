using System.Diagnostics.CodeAnalysis;

namespace RPWebSvrCli.Config;

[ExcludeFromCodeCoverage]
public class LogLevel
{
    public string Default { get; set; } = "Information";
}
