using System.ComponentModel.DataAnnotations;

namespace blog_api_dev.Models.Tech_Article
{
    public class Tech_Article_Post
    {
        [Required]
        public int tech_id { get; set; }
        [Required]
        public int article_id { get; set; }
    }
}