using System;
using System.Threading.Tasks;
using blog_api_dev.Broken.Services.User;
using blog_api_dev.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace blog_api_dev.Controllers
{
  [Route("api/v1/user")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly UserServices _services;
    
    public UserController(IConfiguration configuration)
    {
      _services = new UserServices(configuration);
    }

    [HttpGet]
    public async Task<JsonResult> GetListUsers()
    {
      return await _services.Get(false);
    }

    [HttpGet("{id}")]
    public async Task<Object> GetUserById([FromRoute]string id)
    {
      var user = await _services.Get(id);
      return new JsonResult(user.Value);
    }

    [HttpPost]
    public async Task<Object> PostNewUser([FromBody]UserPost user)
    {
      return await _services.Post(user);
    }
  }
}