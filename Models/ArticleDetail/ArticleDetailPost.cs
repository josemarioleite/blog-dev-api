using System.ComponentModel.DataAnnotations;

namespace blog_api_dev.Models.ArticleDetail
{
    public class ArticleDetailPost
    {
        [Required]
        public string article_id { get; set; }
        [Required]
        public string text { get; set; }
    }
}