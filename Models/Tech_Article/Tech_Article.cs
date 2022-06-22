using System.ComponentModel.DataAnnotations;
using blog_api_dev.Models.Tech;

namespace blog_api_dev.Models.Tech_Article
{
    public class Tech_Article
    {
        [Key]
        public int id { get; set; }
        public int tech_id { get; set; }
        public int article_id { get; set; }
    }
}