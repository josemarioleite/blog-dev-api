using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace blog_api_dev.Models.Article
{
    public class ArticlePost
    {
        [Required]
        [MaxLength(100, ErrorMessage = "O máximo de caracteres suportado é 100!")]
        public string title { get; set; }
        [Required(ErrorMessage = "O campo notion_id é obrigatório!")]
        [MaxLength(40, ErrorMessage = "O máximo de caracteres suportado é 40!")]
        public string notion_id { get; set; }
        [Required]
        public DateTime datePublication { get; set; } = DateTime.Now;
        [JsonIgnore]
        public Models.Tech.Technology[] technologies { get; set; }
        public Boolean isVisible { get; set; } = true;
    }
}