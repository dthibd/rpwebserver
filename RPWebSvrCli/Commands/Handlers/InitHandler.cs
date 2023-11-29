using System.IO.Abstractions;
using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RPWebSvrCli.Commands.Requests;
using RPWebSvrCli.Config;

namespace RPWebSvrCli.Commands.Handlers;

public class InitHandler : IRequestHandler<InitRequest>
{
    public ILogger<InitHandler> Logger { get; }
    
    public IFileSystem FileSystem { get; }
    
    public IOptions<Settings> SettingsOptions { get; }
    
    public readonly string SettingsFilename = "appsettings.json";
    
    public InitHandler(
        ILogger<InitHandler> logger,
        IFileSystem fileSystem,
        IOptions<Settings> settingsOptions
        )
    {
        Logger = logger;
        FileSystem = fileSystem;
        SettingsOptions = settingsOptions;
    }
    
    public Task Handle(InitRequest request, CancellationToken cancellationToken)
    {
        Logger.LogDebug($"init request path : {request.Path}");

        try
        {
            var filePath = FileSystem.Path.Join(request.Path, SettingsFilename);

            if (FileSystem.File.Exists(filePath))
            {
                Logger.LogInformation($"{SettingsFilename} already exists at {request.Path}");
                return Task.CompletedTask;
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(SettingsOptions.Value, Formatting.Indented);

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
