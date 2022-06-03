using System.Threading.Tasks;
using blog_api_dev.Broken.Services.Article;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace blog_api_dev.Controllers
{
  [Route("api/v1/article/detail")]
    [ApiController]
    public class ArticleDetailController : ControllerBase
    {
        private readonly ArticleDetailServices _services;
        
        public ArticleDetailController(IConfiguration configuration)
        {
            _services = new ArticleDetailServices(configuration);
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> GetArticleDetailById([FromRoute]string id)
        {
            return await _services.Get(id);
        }
        // [HttpPost]
        // public async Task<Object> PostNewArticle([FromBody]ArticleDetailPost articlePost)
        // {
        //     ArticleDetail article = new ArticleDetail
        //     {
        //         article_id = ObjectId.Parse(articlePost.article_id),
        //         text = articlePost.text
        //     };
        //     return await _services.Post(article);
        // }
    }
}