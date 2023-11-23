using Microsoft.AspNetCore.Mvc;
using RPWebServer.Services;

namespace RPWebServer.Controllers;

[Route("[controller]")]
public class ComponentsController : Controller
{
    public ILogger<ComponentsController> Logger { get; }
    public IComponentsMappingService ComponentsMappingService { get; }

    public ComponentsController(
        ILogger<ComponentsController> logger,
        IComponentsMappingService componentsMappingService
        )
    {
        Logger = logger;
        ComponentsMappingService = componentsMappingService;
    }

    [HttpGet("{**url}")]
    public IActionResult Index(string url)
    {
        try
        {
            var provider = ComponentsMappingService.GetProviderForUrl(url);

            if (provider == null)
            {
                Logger.LogInformation($"no component provider found for url '{url}'");
                return BadRequest();
            }

            Logger.LogInformation($"component provider found : {provider.Id}");

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
