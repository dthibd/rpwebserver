using System.Diagnostics.CodeAnalysis;

namespace RPWebSvrCli.Config;

[ExcludeFromCodeCoverage]
public class Settings
{
    public Logging Logging { get; set; } = new();
    public Server Server { get; set; } = new();
}
