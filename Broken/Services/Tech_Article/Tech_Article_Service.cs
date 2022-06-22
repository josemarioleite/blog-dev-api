using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog_api_dev.Broken.Services.Tech_Article
{
    public class Tech_Article_Service
    {
        private readonly DbContextDatabase _database;
        private readonly IMapper _imapper;

        public Tech_Article_Service(DbContextDatabase database, IMapper imapper = null)
        {
            _database = database;
            _imapper = imapper;
        }

        public async Task<JsonResult> ListTechArticles(int article_id = 0)
        {
            List<Models.Tech_Article.Tech_Article> techArt = new List<Models.Tech_Article.Tech_Article>();
            if (article_id.Equals(0)) {
                techArt = await _database.Tech_Article.AsNoTracking().ToListAsync();
            } else {
                techArt = await _database.Tech_Article.AsNoTracking().Where(obj => obj.article_id.Equals(article_id)).ToListAsync();
            }

            return new JsonResult(techArt);
        }
    }
}