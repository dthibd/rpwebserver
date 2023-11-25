using Grpc.Core;

namespace RPWebServer.Services.GRPC;

public class GetToolVersionService : GetToolVersion.GetToolVersionBase
{
    public ILogger<GetToolVersionService> Logger { get; }
    
    public GetToolVersionService(
        ILogger<GetToolVersionService> logger
        )
    {
        Logger = logger;
    }
    
    public override Task<GetToolVersionResponse> GetToolVersion(GetToolVersionRequest request, ServerCallContext context)
    {
        Logger.LogInformation("GetToolVersion called");
        return Task.FromResult(new GetToolVersionResponse
        {
            Version = "1.0.0"
        });
    }
}
