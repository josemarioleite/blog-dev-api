using System.Threading.Tasks;
using AutoMapper;
using blog_api_dev.Broken;
using blog_api_dev.Broken.Services.Tech_Article;
using Microsoft.AspNetCore.Mvc;

namespace blog_api_dev.Controllers.Tech_Article
{
    [Route("api/v1/tech_article")]
    [ApiController]
    public class Tech_Article_Controller : ControllerBase
    {
        private readonly Tech_Article_Service _techArticle;

        public Tech_Article_Controller(DbContextDatabase database)
        {
            _techArticle = new Tech_Article_Service(database);
        }

        [HttpGet]
        public async Task<JsonResult> ListTechs([FromQuery]int article_id = 0)
        {
            if (ModelState.IsValid)
            {
                return await _techArticle.ListTechArticles(article_id);
            } else {
                return Utils.TypeUtils.ReturnTypeResponseHTTP(false, null, "Preencha todos os campos válidos!");
            }
        }
    }
}