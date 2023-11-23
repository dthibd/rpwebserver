using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebComponentServer.Commands.Responses;
using WebComponentServer.Commands.ReverseProxy;
using WebComponentServer.Commands.ReverseProxy.Clusters;
using WebComponentServer.Commands.ReverseProxy.Routes;
using WebComponentServer.Models.ClusterConfig;
using WebComponentServer.Models.RouteConfig;

namespace WebComponentServer.Controllers;

[Route("[controller]")]
public class ReverseProxyController : Controller
{
    private IMediator _mediator;

    public ReverseProxyController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Routes/Ids")]
    public async Task<IActionResult> ListRouteIds()
    {
        var response = await _mediator.Send<ListRouteIdsRequestResponse>(new ListRouteIdsRequest());
        return Ok(response.Value);
    }

    [HttpGet("Routes")]
    public async Task<IActionResult> ListRoutes()
    {
        var response = await _mediator.Send<ListRoutesRequestResponse>(new ListRoutesRequest());

        return Ok(response.Value);
    }

    [HttpGet("Route/{id}")]
    public async Task<IActionResult> GetRouteById(string id)
    {
        var response = await _mediator.Send<GetRouteByIdResponse>(new GetRouteByIdRequest(id));

        return Ok(response.Value);
    }

    [HttpPost("Route")]
    public async Task<IActionResult> AddRoute([FromBody] RouteConfigDto routeConfigDto)
    {
        var response = await _mediator.Send<AddRouteResponse>(new AddRouteRequest(routeConfigDto));

        if (!response.Succeeded)
        {
            return BadRequest(response.Error);
        }

        return Ok(response.Value);
    }

    [HttpPut("Route")]
    public async Task<IActionResult> UpdateRoute([FromBody] RouteConfigDto routeConfigDto)
    {
        var result = await _mediator.Send<RequestResponse>(new UpdateRouteRequest(routeConfigDto));

        if (!result.Succeeded)
        {
            return BadRequest(result.Error);
        }
        
        return Ok(routeConfigDto);
    }

    [HttpDelete("Route/{id}")]
    public async Task<IActionResult> DeleteRoute(string id)
    {
        var response = await _mediator.Send<RequestResponse>(new DeleteRouteRequest(id));

        if (!response.Succeeded)
        {
            return BadRequest(response.Error);
        }

        return Ok();
    }

    
    [HttpGet("Clusters/Ids")]
    public async Task<IActionResult> ListClusterIds()
    {
        var response = await _mediator.Send<ListClusterIdsResponse>(new ListClusterIdsRequest());

        return Ok(response.Value);
    }

    [HttpGet("Clusters")]
    public async Task<IActionResult> ListClusters()
    {
        var response = await _mediator.Send<ListClustersResponse>(new ListClustersRequest());

        return Ok(response.Value);
    }
    
    [HttpGet("cluster/{id}")]
    public async Task<IActionResult> GetClusterById(string id)
    {
        var response = await _mediator.Send<GetClusterByIdResponse>(new GetClusterByIdRequest(id));

        return Ok(response.Value);
    }

    [HttpPost("cluster")]
    public async Task<IActionResult> AddCluster([FromBody] ClusterConfigDto clusterConfigDto)
    {
        var response = await _mediator.Send<AddClusterResponse>(new AddClusterRequest(clusterConfigDto));

        if (!response.Succeeded)
        {
            return BadRequest(response.Error);
        }

        return Ok(response.Value);
    }

    [HttpPut("cluster")]
    public async Task<IActionResult> UpdateCluster([FromBody] ClusterConfigDto clusterConfig)
    {
        var response = await _mediator.Send<UpdateClusterResponse>(new UpdateClusterRequest(clusterConfig));

        if (!response.Succeeded)
        {
            return BadRequest(response.Error);
        }
        
        return Ok(response.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        await _mediator.Send(new RefreshReverseProxyRequest());
        return Ok();
    }
}

