using System.Threading.Tasks;
using blog_api_dev.Broken;
using blog_api_dev.Models.User;
using blog_api_dev.Statics;
using blog_api_dev.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;

namespace blog_api_dev.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private DbContextDatabase _database;

        public AuthController(IConfiguration configuration)
        {
            _database = new DbContextDatabase(configuration);
        }
    }
}