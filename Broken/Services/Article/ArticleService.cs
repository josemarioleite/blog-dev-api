using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using blog_api_dev.Broken.Services.Tech_Article;
using blog_api_dev.Broken.Services.Technology;
using blog_api_dev.Models.Article;
using blog_api_dev.Models.Tech_Article;
using blog_api_dev.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using remarsemanal.Model;

namespace blog_api_dev.Broken.Services.Article
{
  public class ArticleService
    {
        private readonly DbContextDatabase _database;
        private readonly IMapper _imapper;

        public ArticleService(DbContextDatabase database, IMapper imapper = null)
        {
            _database = database;
            _imapper = imapper;
        }

        public async Task<JsonResult> ListHiddenArticles(
            bool hasParam = false,
            string id = null,
            int pageIndex = 1,
            int pageSize = 5
        )
        {
            List<Models.Article.Article> article = new List<Models.Article.Article>();

            if (hasParam) {
                article = await _database.Article.AsNoTracking().Where(obj => obj.id.Equals(int.Parse(id)) && obj.isVisible == false).ToListAsync();
            } else {
                article = await _database.Article.AsNoTracking().Where(obj => obj.isVisible == false).ToListAsync();
            }

            int rowCount = article.Count;
            article = await PaginatedList<Models.Article.Article>.CreateAsync(article, pageIndex, pageSize);

            var query = from a in article select new Models.Article.ArticleModelDTO
                    {
                        id = a.id,
                        title = a.title,
                        datePublication = a.datePublication,
                        notion_id = a.notion_id,
                        isVisible = a.isVisible,
                        technologies = returnTechnologies(a.id)
                    };
            
            return new JsonResult(new
            {
                rowCount = rowCount,
                article = query.OrderByDescending(c => c.datePublication)
            });
        }

        public async Task<JsonResult> ListArticles(
            bool hasParam = false,
            bool onlyRowCount = false,
            bool onlyQuery = false,
            int pageIndex = 1,
            int pageSize = 5,
            string id = null,
            string filter = null)
        {
            int rowCount = 0;
            List<Models.Article.Article> article = new List<Models.Article.Article>();

            if (onlyQuery && !hasParam && !string.IsNullOrEmpty(id))
            {
                article = await _database.Article.AsNoTracking().Where(obj => obj.id.Equals(int.Parse(id))).OrderByDescending(obj => obj.datePublication).ToListAsync();
            } else {
                article = await _database.Article.AsNoTracking().OrderByDescending(obj => obj.datePublication).ToListAsync();
            }

            if (string.IsNullOrEmpty(filter) || filter.ToLower() == "undefined") {
                filter = null;
            }
            
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                article = article.Where(t => (t.title != null && t.title.ToLower().RemoveWordsSpecials().Contains(filter))).ToList();
            }

            rowCount = article.Count;

            if (onlyRowCount)
            {
                return new JsonResult(rowCount);
            }

            article = await PaginatedList<Models.Article.Article>.CreateAsync(article, pageIndex, pageSize);

            var query = from a in article select new Models.Article.ArticleModelDTO
                    {
                        id = a.id,
                        title = a.title,
                        datePublication = a.datePublication,
                        notion_id = a.notion_id,
                        isVisible = a.isVisible,
                        technologies = returnTechnologies(a.id)
                    };

            if (onlyQuery && hasParam && !string.IsNullOrEmpty(id))
            {
                if (query != null) {
                    List<ArticleModelDTO> artDTO = new List<ArticleModelDTO>();
                    foreach (var art in query)
                    {
                        foreach (var tech in art.technologies)
                        {
                            if (tech.id.Equals(int.Parse(id))) {
                                artDTO.Add(art);
                            }
                        }
                    }
                    query = artDTO;
                }
            }

            return new JsonResult(new
            {
                rowCount = rowCount,
                article = query.OrderByDescending(c => c.datePublication)
            });
        }

        public List<Models.Tech.Technology> returnTechnologies (int article_id = 0)
        {
            var tech_art = _database.Tech_Article
                                            .AsNoTracking()
                                            .Where(obj => obj.article_id.Equals(article_id))
                                            .ToList();

            List<Models.Tech.Technology> technologies = new List<Models.Tech.Technology>();
            if (tech_art != null) {
                foreach (var item in tech_art)
                {
                    technologies.Add(_database.Technology.AsNoTracking().FirstOrDefault(obj => obj.id.Equals(item.tech_id)));
                }
            }

            return technologies;
        }

        public async Task<JsonResult> AddNewArticle(ArticlePost article)
        {
            if (await _database.Article.FirstOrDefaultAsync(u => u.title.ToLower() == article.title.ToLower()) != null) {
                return TypeUtils.ReturnTypeResponseHTTP(false, null, "Este artigo já foi publicado");
            }

            if (article.technologies == null) {
                return TypeUtils.ReturnTypeResponseHTTP(false, null, "É necessário informar a que tipo de tecnologia(s) se refere");
            }
            else {
                foreach (var item in article.technologies)
                {
                    var exists = await _database.Technology.FirstOrDefaultAsync(obj => obj.id.Equals(item.id));

                    if (exists == null) {
                        return TypeUtils.ReturnTypeResponseHTTP(false, null, $"{item.name}(ID: {item.id}) não existe no sistema");
                    }                   
                }
            }
            
            var newArticle =_imapper.Map<Models.Article.Article>(article);
            await _database.Article.AddAsync(newArticle);
            try {
                await _database.SaveChangesAsync();

                try {
                    foreach (var item in article.technologies)
                    {    
                        var tech_art = new Tech_Article_Post
                        {
                            tech_id = item.id,
                            article_id = newArticle.id
                        };

                        var newTechArticle = _imapper.Map<Models.Tech_Article.Tech_Article>(tech_art);
                        await _database.Tech_Article.AddAsync(newTechArticle);
                        await _database.SaveChangesAsync();
                    }
                    
                } catch (Exception ex) {
                    return TypeUtils.ReturnTypeResponseHTTP(false, ex);
                }

                return TypeUtils.ReturnTypeResponseHTTP(true);
            } catch (Exception ex) {
                return TypeUtils.ReturnTypeResponseHTTP(false, ex);
            }
        }

        public async Task<JsonResult> HiddenShowArticle(Models.Article.Article articleChange, string id = null)
        {
            if (int.Parse(id) != articleChange.id) {
                return TypeUtils.ReturnTypeResponseHTTP(false, null, "ID informado não é igual ao Id do artigo enviado para atualização");
            }

            if (string.IsNullOrEmpty(id) || int.Parse(id) == 0) {
                return TypeUtils.ReturnTypeResponseHTTP(false, null, "Insira o ID do artigo");
            }

            if (articleChange == null) {
                return TypeUtils.ReturnTypeResponseHTTP(false, null, "Artigo nulo, insira as informações para atualização");
            }

            // var article = await _database.Article.FindAsync(articleChange.id);
            var article = await _database.Article.AsNoTracking().FirstOrDefaultAsync(obj => obj.id.Equals(int.Parse(id)));

            if (article.isVisible.Equals(articleChange.isVisible)) {
                return TypeUtils.ReturnTypeResponseHTTP(false, null, "Não houve alterações no artigo");
            }

            if (article == null) {
                return TypeUtils.ReturnTypeResponseHTTP(false, null, "Artigo não encontrado");
            } else {
                try {
                    _database.Entry(articleChange).State = EntityState.Modified;
                    _database.Entry(articleChange).Property(p => p.id).IsModified = false;
                    _database.Entry(articleChange).Property(p => p.datePublication).IsModified = false;
                    _database.Entry(articleChange).Property(p => p.notion_id).IsModified = false;
                    _database.Entry(articleChange).Property(p => p.title).IsModified = false;

                    await _database.SaveChangesAsync();
                    return TypeUtils.ReturnTypeResponseHTTP(true, null, "Artigo alterado com sucesso!");
                } catch (Exception ex) {
                    return TypeUtils.ReturnTypeResponseHTTP(false, ex);
                }
            }
        }
    }
}