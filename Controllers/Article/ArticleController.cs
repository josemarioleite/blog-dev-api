using System.Threading.Tasks;
using AutoMapper;
using blog_api_dev.Broken;
using blog_api_dev.Broken.Services.Article;
using blog_api_dev.Models.Article;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_api_dev.Controllers.Article
{
    [Route("api/v1/article")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleService articleService;

        public ArticleController(DbContextDatabase database, IMapper mapper)
        {
            articleService = new ArticleService(database, mapper);
        }

        [HttpPut("hidden/{id}")]
        [Authorize]
        public async Task<JsonResult> UpdateArticle([FromBody]Models.Article.Article article, [FromRoute]string id = null)
        {
            if (ModelState.IsValid) {
                return await articleService.HiddenShowArticle(article, id);
            } else {
                return Utils.TypeUtils.ReturnTypeResponseHTTP(false, null, "Preencha todos os campos válidos!");
            }
        }

        [HttpGet("hidden")]
        [Authorize]
        public async Task<JsonResult> ListHiddenArticles (
            [FromQuery]bool hasParam = false,
            [FromQuery]string id = null,
            [FromQuery]int pageIndex = 1,
            [FromQuery]int pageSize = 5
        )
        {
            if (ModelState.IsValid) {
                return await articleService.ListHiddenArticles(
                    hasParam: hasParam,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    id: id
                );
            } else {
                return Utils.TypeUtils.ReturnTypeResponseHTTP(false, null, "Preencha todos os campos válidos!");
            }
        }

        [HttpGet]
        public async Task<JsonResult> ListArticles(
            [FromQuery]bool hasParam = false,
            [FromQuery]bool onlyRowCount = false,
            [FromQuery]bool onlyQuery = false,
            [FromQuery]int pageIndex = 1,
            [FromQuery]int pageSize = 5,
            [FromQuery]string id = null,
            [FromQuery]string filter = null
        )
        {
            if (ModelState.IsValid)
            {
                return await articleService.ListArticles(
                    hasParam: hasParam,
                    onlyRowCount: onlyRowCount,
                    onlyQuery: onlyQuery,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    id: id,
                    filter: filter
                );
            } else {
                return Utils.TypeUtils.ReturnTypeResponseHTTP(false, null, "Preencha todos os campos válidos!");
            }
        }

        [HttpPost]
        public async Task<ActionResult> NewArticle([FromBody]ArticlePost techArticle)
        {
            if (ModelState.IsValid)
            {
                return await articleService.AddNewArticle(techArticle);
            } else {
                return Utils.TypeUtils.ReturnTypeResponseHTTP(false, null, "Preencha todos os campos válidos!");
            }
        }
    }
}