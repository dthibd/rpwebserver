using Microsoft.Extensions.Logging;

namespace RPWebSvrCli.Services;

public class Worker : IWorker
{
    public ILogger<IWorker> Logger { get; }
    
    public ITextOutput TextOutput { get; }
    
    public Worker(
        ILogger<IWorker> logger,
        ITextOutput textOutput
        )
    {
        Logger = logger;
        TextOutput = textOutput;
    }

    public void HandleCommandLineOptions(CommandLineOptions options)
    {
        if (options.ToolVersion)
        {
            TextOutput.WriteLine("Tool version: 0.0.1");
        }
    }
}
