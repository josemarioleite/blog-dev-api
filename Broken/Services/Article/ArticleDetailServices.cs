using System;
using System.Linq;
using System.Threading.Tasks;
using blog_api_dev.Models.ArticleDetail;
using blog_api_dev.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace blog_api_dev.Broken.Services.Article
{
    public class ArticleDetailServices
    {
        private readonly DbContextDatabase _database;

        public ArticleDetailServices(IConfiguration configuration)
        {
            _database = new DbContextDatabase(configuration);
        }

        public async Task<JsonResult> Get(string id)
        {
            try {
                var filterUser = Builders<ArticleDetail>.Filter.Eq(obj => obj.article_id, ObjectId.Parse(id));
                var user = await _database.dbClient.GetCollection<ArticleDetail>("Article_Detail").Find(filterUser).SingleAsync();

                return new JsonResult(user);
            } catch (Exception ex) {
                return TypeUtils.ReturnTypeResponseHTTP(false, ex);
            }
        }

        public async Task<JsonResult> Post<T>(T obj)
        {
            try {
                var query = _database.dbClient.GetCollection<ArticleDetail>("ArticleDetail");
                await query.InsertOneAsync(obj.como<ArticleDetail>());
                return TypeUtils.ReturnTypeResponseHTTP();
            } catch (Exception ex) {
                return TypeUtils.ReturnTypeResponseHTTP(false, ex);
            }
        }
    }
}