using System.Threading.Tasks;
using blog_api_dev.Broken;
using blog_api_dev.Broken.Services.User;
using blog_api_dev.Models;
using blog_api_dev.Models.User;
using blog_api_dev.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_api_dev.Controllers.User
{
    [Route("api/v1/auth")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly UserService userService;
        private readonly JwtSettings _jwtSettings;

        public UserAuthController(DbContextDatabase database, JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
            userService = new UserService(database, null, jwtSettings);
        }

        [HttpGet("token")]
        [Authorize]
        public void TokenValido()
        {
            return;
        }

        [HttpPost("login")]
        public async Task<ActionResult> InsertNewUser([FromBody]UserAuth user)
        {
            if (ModelState.IsValid)
            {
                return await userService.AuthUser(user, _jwtSettings);
            } else {
                return TypeUtils.ReturnTypeResponseHTTP(false, null, "Preencha todos os campos válidos!");
            }
        }
    }
}