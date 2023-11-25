using System.Diagnostics.CodeAnalysis;

namespace RPWebSvrCli.Services;

[ExcludeFromCodeCoverage]
public class ConsoleTextOutput : ITextOutput
{
    public void WriteLine(string text)
    {
        Console.WriteLine(text);
    }
}