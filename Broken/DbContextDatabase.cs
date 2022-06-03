using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace blog_api_dev.Broken
{
  public class DbContextDatabase
    {
        private readonly IConfiguration _configuration;
        public IMongoDatabase dbClient { get; set; }
        public DbContextDatabase(IConfiguration configuration)
        {
            _configuration = configuration;
            dbClient = new MongoClient(_configuration.GetConnectionString("conexaoMongoDB")).GetDatabase("blog-dev");
        }
    }
}