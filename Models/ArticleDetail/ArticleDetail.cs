using MongoDB.Bson;

namespace blog_api_dev.Models.ArticleDetail
{
    public class ArticleDetail
    {
        public ObjectId id { get; set; }
        public ObjectId article_id { get; set; }
        public string notion_page_id { get; set; }
    }
}