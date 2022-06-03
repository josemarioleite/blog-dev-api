using System;
using System.Linq;
using System.Threading.Tasks;
using blog_api_dev.Interface;
using blog_api_dev.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace blog_api_dev.Broken.Services.User
{
    public class UserServices
    {
        private readonly DbContextDatabase _database;

        public UserServices(IConfiguration configuration)
        {
            _database = new DbContextDatabase(configuration);
        }

        public async Task<JsonResult> Get(bool hasParam = false, string param = null)
        {
            try {
                var user = await _database.dbClient.GetCollection<Models.User.User>("User").AsQueryable().ToListAsync();

                return new JsonResult(user.OrderBy(obj => obj.name));
            } catch (Exception ex) {
                return TypeUtils.ReturnTypeResponseHTTP(false, ex);
            }
        }

        public async Task<JsonResult> Get(string id)
        {
            try {
                var filterUser = Builders<Models.User.User>.Filter.Eq(obj => obj.id, ObjectId.Parse(id));
                var user = await _database.dbClient.GetCollection<Models.User.User>("User").Find(filterUser).SingleAsync();

                return new JsonResult(user);
            } catch (Exception ex) {
                return TypeUtils.ReturnTypeResponseHTTP(false, ex);
            }
        }

        public async Task<JsonResult> Post<T>(T obj)
        {
            try {
                var query = _database.dbClient.GetCollection<Models.User.UserPost>("User");
                await query.InsertOneAsync(obj.como<Models.User.UserPost>());
                return TypeUtils.ReturnTypeResponseHTTP();
            } catch (Exception ex) {
                return TypeUtils.ReturnTypeResponseHTTP(false, ex);
            }
        }
  }
}