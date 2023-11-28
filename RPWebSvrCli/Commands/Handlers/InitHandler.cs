using System.IO.Abstractions;
using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RPWebSvrCli.Commands.Requests;
using RPWebSvrCli.Models;

namespace RPWebSvrCli.Commands.Handlers;

public class InitHandler : IRequestHandler<InitRequest>
{
    public ILogger<InitHandler> Logger { get; }
    
    public IFileSystem FileSystem { get; }
    
    public InitHandler(
        ILogger<InitHandler> logger,
        IFileSystem fileSystem
        )
    {
        Logger = logger;
        FileSystem = fileSystem;
    }
    
    public Task Handle(InitRequest request, CancellationToken cancellationToken)
    {
        Logger.LogDebug($"init request path : {request.Path}");

        try
        {
            var emptyConfig = new Configuration();

            var filename = "RPWebSvrCli.json";
            var filePath = FileSystem.Path.Join(request.Path, filename);

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(emptyConfig, Formatting.Indented);

            var file = FileSystem.FileStream.New(filePath, FileMode.Create);
            file.Write(Encoding.UTF8.GetBytes(json));
            file.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Task.FromException(e);
        }

        return Task.CompletedTask;
    }
}
