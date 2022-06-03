using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using System;
using blog_api_dev.Models.Tech;
using System.Collections.Generic;

namespace blog_api_dev.Models.Article
{
    public class Article
    {
        public ObjectId id { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public DateTime createdAt { get; set; }
        [Required]
        public List<Technology> technology { get; set; }
    }

    public class ArticleModel
    {
        public ObjectId id { get; set; }
        [Required]
        public ObjectId[] technology { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public DateTime createdAt { get; set; }
    }
}