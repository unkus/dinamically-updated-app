using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Resources;

namespace UpdateService.Controllers;

[ApiController]
[Route("[controller]")]
public class UpdaterController : ControllerBase
{

    private readonly ILogger<UpdaterController> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly string _version;

    public UpdaterController(ILogger<UpdaterController> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;

        try
        {
            using var sr = new StreamReader("MyApp/MyApp.version");
            _version = sr.ReadToEndAsync().Result;
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Route("LatestVersion")]
    public string? LatestVersion()
    {
        _logger.LogDebug($"Latest version requested ({_version})");
        return _version;
    }

    [HttpGet]
    [Route("Latest")]
    public PhysicalFileResult Latest()
    {
        _logger.LogDebug("Latest app requested");
        var filepath = Path.Combine(_environment.ContentRootPath, "MyApp", "MyApp.zip");
        return PhysicalFile(filepath, "application/zip");
    }
}