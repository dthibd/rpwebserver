namespace RPWebSvrCli.Config;

public class Settings
{
    public Logging Logging { get; set; } = new();
    public Server Server { get; set; } = new();
}
