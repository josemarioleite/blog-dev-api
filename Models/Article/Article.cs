using System.ComponentModel.DataAnnotations;
using System;

namespace blog_api_dev.Models.Article
{
  public class Article
    {
      [Key]
      public int id { get; set; }
      public string title { get; set; }
      public string notion_id { get; set; }
      public DateTime datePublication { get; set; }
      public Boolean isVisible { get; set; }
    }
}