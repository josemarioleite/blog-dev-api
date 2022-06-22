using System.Collections.Generic;
using blog_api_dev.Models.Tech;

namespace blog_api_dev.Models.Article
{
    public class ArticleModelDTO : Article
    {
        public List<Technology> technologies { get; set; }
    }
}