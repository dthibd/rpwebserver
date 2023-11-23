using MediatR;
using Microsoft.AspNetCore.Mvc;
using RPWebServer.Commands.Responses;
using RPWebServer.Commands.ReverseProxy;
using RPWebServer.Commands.ReverseProxy.Clusters;
using RPWebServer.Commands.ReverseProxy.Routes;
using RPWebServer.Models.ClusterConfig;
using RPWebServer.Models.RouteConfig;

namespace RPWebServer.Controllers;

[Route("[controller]")]
public class ReverseProxyController : Controller
{
    public IMediator Mediator { get; }

    public ReverseProxyController(
        IMediator mediator)
    {
        Mediator = mediator;
    }

    [HttpGet("Routes/Ids")]
    public async Task<IActionResult> ListRouteIds()
    {
        var response = await Mediator.Send<ListRouteIdsRequestResponse>(new ListRouteIdsRequest());
        return Ok(response.Value);
    }

    [HttpGet("Routes")]
    public async Task<IActionResult> ListRoutes()
    {
        var response = await Mediator.Send<ListRoutesRequestResponse>(new ListRoutesRequest());

        return Ok(response.Value);
    }

    [HttpGet("Route/{id}")]
    public async Task<IActionResult> GetRouteById(string id)
    {
        var response = await Mediator.Send<GetRouteByIdResponse>(new GetRouteByIdRequest(id));

        return Ok(response.Value);
    }

    [HttpPost("Route")]
    public async Task<IActionResult> AddRoute([FromBody] RouteConfigDto routeConfigDto)
    {
        var response = await Mediator.Send<AddRouteResponse>(new AddRouteRequest(routeConfigDto));

        if (!response.Succeeded)
        {
            return BadRequest(response.Error);
        }

        return Ok(response.Value);
    }

    [HttpPut("Route")]
    public async Task<IActionResult> UpdateRoute([FromBody] RouteConfigDto routeConfigDto)
    {
        var result = await Mediator.Send<RequestResponse>(new UpdateRouteRequest(routeConfigDto));

        if (!result.Succeeded)
        {
            return BadRequest(result.Error);
        }
        
        return Ok(routeConfigDto);
    }

    [HttpDelete("Route/{id}")]
    public async Task<IActionResult> DeleteRoute(string id)
    {
        var response = await Mediator.Send<RequestResponse>(new DeleteRouteRequest(id));

        if (!response.Succeeded)
        {
            return BadRequest(response.Error);
        }

        return Ok();
    }

    
    [HttpGet("Clusters/Ids")]
    public async Task<IActionResult> ListClusterIds()
    {
        var response = await Mediator.Send<ListClusterIdsResponse>(new ListClusterIdsRequest());

        return Ok(response.Value);
    }

    [HttpGet("Clusters")]
    public async Task<IActionResult> ListClusters()
    {
        var response = await Mediator.Send<ListClustersResponse>(new ListClustersRequest());

        return Ok(response.Value);
    }
    
    [HttpGet("cluster/{id}")]
    public async Task<IActionResult> GetClusterById(string id)
    {
        var response = await Mediator.Send<GetClusterByIdResponse>(new GetClusterByIdRequest(id));

        if (!response.Succeeded)
        {
            return NotFound();
        }
        
        return Ok(response.Value);
    }

    [HttpPost("cluster")]
    public async Task<IActionResult> AddCluster([FromBody] ClusterConfigDto clusterConfigDto)
    {
        var response = await Mediator.Send<AddClusterResponse>(new AddClusterRequest(clusterConfigDto));

        if (!response.Succeeded)
        {
            return BadRequest(response.Error);
        }

        return Ok(response.Value);
    }

    [HttpPut("cluster")]
    public async Task<IActionResult> UpdateCluster([FromBody] ClusterConfigDto clusterConfig)
    {
        var response = await Mediator.Send<UpdateClusterResponse>(new UpdateClusterRequest(clusterConfig));

        if (!response.Succeeded)
        {
            return BadRequest(response.Error);
        }
        
        return Ok(response.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        await Mediator.Send(new RefreshReverseProxyRequest());
        return Ok();
    }
}

