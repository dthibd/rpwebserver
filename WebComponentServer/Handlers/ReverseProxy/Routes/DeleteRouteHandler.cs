using MediatR;
using WebComponentServer.Commands.Responses;
using WebComponentServer.Commands.ReverseProxy.Routes;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServer.Handlers.ReverseProxy.Routes;

public class DeleteRouteHandler : IRequestHandler<DeleteRouteRequest, RequestResponse>
{
    public IRoutesConfigProvider RoutesConfigProvider { get; }

    public DeleteRouteHandler(IRoutesConfigProvider routesConfigProvider)
    {
        RoutesConfigProvider = routesConfigProvider;
    }
    
    public async Task<RequestResponse> Handle(DeleteRouteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            RoutesConfigProvider.Remove(request.Id);

            return new RequestResponse();
        }
        catch (ArgumentException ex)
        {
            return new RequestResponse(false, ex.Message);
        }
    }
}

