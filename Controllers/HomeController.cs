using Microsoft.AspNetCore.Mvc;
using SystemProvisioningPortal.Models;
using SystemProvisioningPortal.Services;

namespace SystemProvisioningPortal.Controllers;

public class HomeController : Controller
{
    private readonly IAzureDevOpsService _azureDevOpsService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IAzureDevOpsService azureDevOpsService, ILogger<HomeController> logger)
    {
        _azureDevOpsService = azureDevOpsService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        // In a real application, these would come from a database or service
        ViewBag.Systems = new List<Models.System>
        {
            new() { Id = "1", Name = "System1" },
            new() { Id = "2", Name = "System2" },
            new() { Id = "3", Name = "System3" }
        };

        ViewBag.Versions = new List<Models.SystemVersion>
        {
            new() { Id = "1", Version = "v1.0.0" }
        };

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Provision(ProvisioningRequest request)
    {
        try
        {
            var result = await _azureDevOpsService.TriggerProvisioningPipelineAsync(request);
            return Json(new { success = true, message = $"{request.Action} pipeline triggered successfully!" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to trigger provisioning pipeline");
            return Json(new { success = false, message = ex.Message });
        }
    }

    public IActionResult Error()
    {
        return View();
    }
}