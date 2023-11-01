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
        return Json($"{url}");
    }
}
