using Microsoft.AspNetCore.Mvc;
using WebComponentServer.Services;

namespace WebComponentServer.Controllers;

[Route("[controller]")]
public class ComponentsController : Controller
{
    private readonly ILogger<ComponentsController> _logger;
    private readonly IComponentsMappingService _componentsMappingService;

    public ComponentsController(
        ILogger<ComponentsController> logger,
        IComponentsMappingService componentsMappingService
        )
    {
        _logger = logger;
        _componentsMappingService = componentsMappingService;
    }

    [HttpGet("{**url}")]
    public IActionResult Index(string url)
    {
        try
        {
            var provider = _componentsMappingService.GetProviderForUrl(url);

            if (provider == null)
            {
                _logger.LogInformation($"no component provider found for url '{url}'");
                return BadRequest();
            }

            _logger.LogInformation($"component provider found : {provider.Id}");

            var result = provider.GetContentForUrl(url);
            if (result.Content == null)
            {
                return Ok();
            }

            return File(result.Content, result.ContentType);
        }
        catch (Exception ex)
        {
            return new StatusCodeResult(404);
        }
    }
}
