using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using blog_api_dev.Models.Article;
using System.Threading.Tasks;
using blog_api_dev.Broken.Services.Article;

namespace api_blog_dev.Controllers
{
    [Route("api/v1/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleServices _services;
        
        public ArticleController(IConfiguration configuration)
        {
            _services = new ArticleServices(configuration);
        }

        public async Task<JsonResult> GetArticleById (
            [FromQuery] string id = null,
            [FromQuery] string filter = null,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 5,
            [FromQuery] bool hasParam = false,
            [FromQuery] bool onlyQuery = false,
            [FromQuery] bool onlyRowCount = false )
        {
            return await _services.GetPagination
            (
                id: id,
                hasParam: hasParam,
                onlyQuery: onlyQuery,
                pageIndex: pageIndex,
                pageSize: pageSize,
                onlyRowCount: onlyRowCount,
                filter: filter
            );
        }

        [HttpPost]
        public async Task<Object> PostNewArticle ([FromBody]ArticlePost articlePost)
        {
            return await _services.Post (articlePost);
        }
    }
}