using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SampleWebApp.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ReleaseController : ControllerBase
{
    [HttpGet]
    [ActionName("Current")]
    public Task<string> GetCurrentAsync()
    {
        return Task.FromResult("2026.1");
    }
}
