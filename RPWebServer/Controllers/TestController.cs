using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace WebComponentServer.Controllers
{
    [Route("[controller]")]
    public class TestController : Controller
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }
        

        [HttpGet("file/{**url}")]
        public IActionResult Index(string url)
        {
            var basePath = "/Users/dannythibaudeau/dev/cobreti-demos/web-components/Components/Angular/Web-Components/dist/web-component";
            var filePath = basePath;

            Regex angularRegex = new Regex(@"^angular/(.*)");
            var match = angularRegex.Match(url);
            if (match.Success)
            {
                filePath = Path.Combine(filePath, match.Groups[1].Value);
            }

            if (!Path.Exists(filePath))
            {
                return NotFound();
            }

            if (url.EndsWith(".ico")) {
                var image = System.IO.File.OpenRead(filePath);
                return File(image, "image/x-icon");
            }
            
            var fileContent = System.IO.File.OpenRead(filePath);
            return File(fileContent, "text/javascript");
        }
    }
}