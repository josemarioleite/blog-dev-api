using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace blog_api_dev.Models.Article
{
    public class ArticlePost
    {
        [Required]
        public string title { get; set; }
        [Required]
        public string user_id { get; set; }
        [Required]
        public DateTime createdAt { get; set; }
        [Required]
        public string technology { get; set; }

        public ArticlePost()
        {
            this.GetDateTimePublication();
        }

        private void GetDateTimePublication () {
            this.createdAt = DateTime.Now;
        }
    }
}