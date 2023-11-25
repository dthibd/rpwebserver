using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using RPWebSvrCli;
using RPWebSvrCli.Commands.Requests;
using RPWebSvrCli.Services;

namespace RPWebServerCliTest.Services;

public class WorkerTest
{
    public Mock<ILogger<IWorker>> LoggerMock { get; } = new();
    
    public Mock<ITextOutput> TextOutputMock { get; } = new();
    
    public Mock<IMediator> MediatorMock { get; } = new();
    
    [Fact]
    public void Construction()
    {
        var worker = new Worker(
            LoggerMock.Object,
            TextOutputMock.Object,
            MediatorMock.Object);

        Assert.Equal(LoggerMock.Object, worker.Logger);
        Assert.Equal(TextOutputMock.Object, worker.TextOutput);
    }

    [Fact]
    public void ToolVersionOptionPresent()
    {
        var worker = new Worker(
            LoggerMock.Object,
            TextOutputMock.Object,
            MediatorMock.Object);
        
        var options = new CommandLineOptions
        {
            ToolVersion = true
        };

        worker.HandleCommandLineOptions(options);
        
        MediatorMock.Verify(m => m.Send(It.IsAny<ShowToolVersionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        
        // TextOutputMock.Verify(m => m.WriteLine(It.IsAny<string>()), Times.Once);
    }
}