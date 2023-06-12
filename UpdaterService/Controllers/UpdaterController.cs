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
    private readonly string _consoleAppVersion;
    private readonly string _wpfAppVersion;

    public UpdaterController(ILogger<UpdaterController> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;

        try
        {
            using var sr = new StreamReader("ConsoleApp/ConsoleApp.version");
            _consoleAppVersion = sr.ReadToEndAsync().Result.Trim();
        }
        catch
        {
            throw;
        }
        try
        {
            using var sr = new StreamReader("WpfApp/WpfApp.version");
            _wpfAppVersion = sr.ReadToEndAsync().Result.Trim();
        }
        catch
        {
            throw;
        }
    }

    [HttpGet]
    [Route("LatestConsoleAppVersion")]
    public string? LatestConsoleAppVersion()
    {
        _logger.LogInformation($"Latest version requested ({_consoleAppVersion})");
        return _consoleAppVersion;
    }

    [HttpGet]
    [Route("LatestConsoleApp")]
    public PhysicalFileResult LatestConsoleApp()
    {
        _logger.LogInformation("Latest app requested");
        var filepath = Path.Combine(_environment.ContentRootPath, "ConsoleApp", "ConsoleApp.zip");
        return PhysicalFile(filepath, "application/zip");
    }

    [HttpGet]
    [Route("LatestWpfAppVersion")]
    public string? LatestWpfAppVersion()
    {
        _logger.LogInformation($"Latest version requested ({_wpfAppVersion})");
        return _wpfAppVersion;
    }

    [HttpGet]
    [Route("LatestWpfApp")]
    public PhysicalFileResult LatestWpfApp()
    {
        _logger.LogInformation("Latest app requested");
        var filepath = Path.Combine(_environment.ContentRootPath, "WpfApp", "WpfApp.zip");
        return PhysicalFile(filepath, "application/zip");
    }
}