using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace blog_api_dev.Models.Tech
{
    public class Technology
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string url_image { get; set; }
    }
}