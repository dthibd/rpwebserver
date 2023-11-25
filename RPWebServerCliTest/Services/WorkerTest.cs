using Microsoft.Extensions.Logging;
using Moq;
using RPWebSvrCli;
using RPWebSvrCli.Services;

namespace RPWebServerCliTest.Services;

public class WorkerTest
{
    public Mock<ILogger<IWorker>> LoggerMock { get; } = new();
    
    public Mock<ITextOutput> TextOutputMock { get; } = new();
    
    [Fact]
    public void Construction()
    {
        var worker = new Worker(LoggerMock.Object, TextOutputMock.Object);

        Assert.Equal(LoggerMock.Object, worker.Logger);
        Assert.Equal(TextOutputMock.Object, worker.TextOutput);
    }

    [Fact]
    public void ToolVersionOptionPresent()
    {
        var worker = new Worker(LoggerMock.Object, TextOutputMock.Object);
        
        var options = new CommandLineOptions
        {
            ToolVersion = true
        };

        worker.HandleCommandLineOptions(options);
        
        TextOutputMock.Verify(m => m.WriteLine(It.IsAny<string>()), Times.Once);
    }
}