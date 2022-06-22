using System.Threading.Tasks;
using AutoMapper;
using blog_api_dev.Broken;
using blog_api_dev.Broken.Services.User;
using blog_api_dev.Models.User;
using blog_api_dev.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_api_dev.Controllers.User
{
    [Route("api/v1/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;
        private readonly SecurityPassword _jwt;

        public UserController(DbContextDatabase database, IMapper mapper)
        {
            userService = new UserService(database, mapper);
            _jwt = new SecurityPassword();
        }

        [HttpGet]
        public async Task<JsonResult> ListUsers()
        {
            int idUser = await _jwt.ReturnIdUserToken(HttpContext);
            return await userService.ListUser(idUser);
        }

        [HttpPost]
        public async Task<ActionResult> InsertNewUser([FromBody]UserPost user)
        {
            return await userService.NewUser(user);
        }
    }
}