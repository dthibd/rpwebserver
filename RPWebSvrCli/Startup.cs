using System.Diagnostics.CodeAnalysis;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RPWebSvrCli.Services;

namespace RPWebSvrCli;

[ExcludeFromCodeCoverage]
public class Startup
{
    public Parser Parser { get; }
    public StringWriter HelpWriter { get; } = new();

    public IServiceProvider ServiceProvider { get; set; }
    
    public IHost App { get; set; }

    public string[] Args { get; }

    public Startup(string[] args)
    {
        Args = args;
        
        Parser = new Parser(c =>
        {
            c.CaseSensitive = false;
            c.AutoVersion = false;
            c.AutoHelp = true;
            c.HelpWriter = HelpWriter;
        });

    }

    public void Init()
    {
        var builder = Host.CreateApplicationBuilder(Args);

        builder.Services
            .AddSingleton<IWorker, Worker>()
            .AddSingleton<ITextOutput, ConsoleTextOutput>()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));

                
        builder.Logging.AddConsole();
        
        App = builder.Build();
    }

    public void Run()
    {
        Parser.ParseArguments<CommandLineOptions>(Args)
            .WithParsed(c =>
            {
                IWorker? worker = App.Services.GetService<IWorker>();

                worker?.HandleCommandLineOptions(c);
            })
            .WithNotParsed(errors =>
            {
                foreach (var error in errors)
                {
                    Console.WriteLine(error);
                }
            });

        var helpText = HelpWriter.ToString();
        if (helpText.Length > 0)
        {
            Console.WriteLine(helpText);
        }
    }
}
