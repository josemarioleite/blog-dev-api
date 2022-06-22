using System.ComponentModel.DataAnnotations;

namespace blog_api_dev.Models.Tech
{
    public class TechnologyPost
    {
        [Required]
        [MaxLength(20)]
        public string name { get; set; }
        [Required]
        public string url_image { get; set; }
    }
}