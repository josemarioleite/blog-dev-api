using System;
using System.Linq;
using System.Threading.Tasks;
using blog_api_dev.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace blog_api_dev.Broken.Services.Technology
{
    public class TechService
    {
        private readonly DbContextDatabase _database;

        public TechService(IConfiguration configuration)
        {
            _database = new DbContextDatabase(configuration);
        }

        public async Task<JsonResult> Get()
        {
            try {
                var user = await _database.dbClient.GetCollection<Models.Tech.Technology>("Technology").AsQueryable().ToListAsync();

                return new JsonResult(user.OrderBy(obj => obj.name));
            } catch (Exception ex) {
                return TypeUtils.ReturnTypeResponseHTTP(false, ex);
            }
        }
    }
}