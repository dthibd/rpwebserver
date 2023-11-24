using System.Diagnostics.CodeAnalysis;
using CommandLine;

namespace RPWebSvrCli;

[ExcludeFromCodeCoverage]
public class CommandLineOptions
{ 
    [Option(Required = false, HelpText = "Display tool version")]
    public Boolean ToolVersion { get; set; }
}
