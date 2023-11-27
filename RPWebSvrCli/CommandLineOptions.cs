using System.Diagnostics.CodeAnalysis;
using CommandLine;

namespace RPWebSvrCli;

[ExcludeFromCodeCoverage]
public class CommandLineOptions
{ 
    [Option(Required = false, HelpText = "Display tool version")]
    public Boolean ToolVersion { get; set; }
    
    [Option(Required = false, HelpText = "Create base configuration file")]
    public Boolean Init { get; set; }
}
