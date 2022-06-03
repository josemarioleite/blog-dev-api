using System.Threading.Tasks;
using blog_api_dev.Broken.Services.Technology;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace blog_api_dev.Controllers
{
  [Route("api/v1/technology")]
  [ApiController]
  public class TechnologyController : ControllerBase
  {
    private readonly TechService _services;
        
    public TechnologyController(IConfiguration configuration)
    {
      _services = new TechService(configuration);
    }

    [HttpGet]
    public async Task<JsonResult> GetTech()
    {
      return await _services.Get();
    }
  }
}