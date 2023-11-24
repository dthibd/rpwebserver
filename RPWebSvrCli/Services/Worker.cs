using Microsoft.Extensions.Logging;

namespace RPWebSvrCli.Services;

public class Worker : IWorker
{
    public ILogger<IWorker> Logger { get; }    
    
    public Worker(ILogger<IWorker> logger)
    {
        Logger = logger;
    }

    public void HandleCommandLineOptions(CommandLineOptions options)
    {
        if (options.ToolVersion)
        {
            Console.WriteLine("Tool Version : 0.0.1");
        }
    }
}
