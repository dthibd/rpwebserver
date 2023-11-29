using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RPWebSvrCli.Config;
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

        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);
        
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));

        builder.Services
            .AddTransient<IFileSystem, FileSystem>()
            .AddSingleton<IWorker, Worker>()
            .AddSingleton<ITextOutput, ConsoleTextOutput>();

        builder.Services
            .Configure<Settings>(builder.Configuration);
                
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
