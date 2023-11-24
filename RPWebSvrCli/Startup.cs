using System.Diagnostics.CodeAnalysis;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace RPWebSvrCli;

[ExcludeFromCodeCoverage]
public class Startup
{
    public Parser Parser { get; }
    public StringWriter HelpWriter { get; } = new();

    public IServiceProvider ServiceProvider { get; set; }

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
        InitServices();
    }

    public void Run()
    {
        Parser.ParseArguments<CommandLineOptions>(Args)
            .WithParsed(c =>
            {
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

    private void InitServices()
    {
        var services = new ServiceCollection();

        ServiceProvider = services.BuildServiceProvider();
    }
}
