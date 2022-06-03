using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using blog_api_dev.Models.Article;
using blog_api_dev.Models.User;
using blog_api_dev.Statics;
using blog_api_dev.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using blog_api_dev.Models.Pagination;
using remarsemanal.Model;

namespace blog_api_dev.Broken.Services.Article
{
    public class ArticleServices
    {
        private readonly DbContextDatabase _database;

        public ArticleServices(IConfiguration configuration)
        {
            _database = new DbContextDatabase(configuration);
        }

        public async Task<JsonResult> GetPagination(
            bool hasParam = false,
            bool onlyRowCount = false,
            bool onlyQuery = false,
            int pageIndex = 0,
            int pageSize = 5,
            string id = null,
            string filter = null )
        {
            int rowCount = 0;
            List<ArticleModel> article = new List<ArticleModel>();

            if (onlyQuery && !hasParam && !string.IsNullOrEmpty(id)) {
                var filterArticle = Builders<ArticleModel>.Filter.Eq(obj => obj.id, ObjectId.Parse(id));
                article = await _database.dbClient.GetCollection<ArticleModel>("Article").Find(filterArticle).ToListAsync();
            }
            else {
                article = await _database.dbClient.GetCollection<ArticleModel>("Article").AsQueryable().ToListAsync();
            }
            
            if (string.IsNullOrEmpty(filter) || filter.ToLower() == "undefined") {
                filter = null;
            }
            
            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower();
                article = article.Where(t => (t.title != null && t.title.ToLower().RemoveWordsSpecials().Contains(filter))).ToList();
            }

            if (onlyQuery && hasParam)
            {
                List<ArticleModel> articleModel = new List<ArticleModel>();
                foreach (var item in article)
                {
                    foreach (var tech in item.technology)
                    {
                        if (tech.Equals(ObjectId.Parse(id)))
                        {
                            articleModel.Add(item);
                        };
                    }
                }
                article.Clear();
                article = articleModel;
            }

            rowCount = article.Count;

            if (onlyRowCount)
            {
                return new JsonResult(rowCount);
            }

            article = await PaginatedList<ArticleModel>.CreateAsync(article, pageIndex, pageSize);

            var query =
                    from a in article
                    select new Models.Article.Article
                    {
                        id = a.id,
                        title = a.title,
                        createdAt = a.createdAt,
                        technology = returnTechnologies(a.technology)
                    };

            return new JsonResult(new PaginationModel
            {
                rowCount = rowCount,
                article = query.OrderByDescending(c => c.createdAt)
            });
        }

        public List<Models.Tech.Technology> returnTechnologies (ObjectId[] obj)
        {
            List<Models.Tech.Technology> tech = new List<Models.Tech.Technology>();
            if (obj != null)
            {
                foreach (var id in obj)
                {
                    var filter = Builders<Models.Tech.Technology>.Filter.Eq(obj => obj.id, id);
                    var technologies = _database.dbClient.GetCollection<Models.Tech.Technology>("Technology").Find(filter).Single();
                    tech.Add(technologies.como<Models.Tech.Technology>());
                }
            }
            return tech;
        }

        public async Task<JsonResult> Post<T>(T obj)
        {
            try {
                var query = _database.dbClient.GetCollection<ArticlePost>("Article");

                if (isArray(obj)) {
                    var post = obj.como<List<ArticlePost>>();
                    await query.InsertManyAsync(post);
                } else {
                    await query.InsertOneAsync(obj.como<ArticlePost>());
                }
                return TypeUtils.ReturnTypeResponseHTTP();
            } catch (Exception ex) {
                return TypeUtils.ReturnTypeResponseHTTP(false, ex);
            }
        }

        private bool isArray (object obj)
        {
            return obj.GetType().IsArray;
        }
    }
}
