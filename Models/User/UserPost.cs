using System;
using System.ComponentModel.DataAnnotations;

namespace blog_api_dev.Models.User
{
    public class UserPost
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string nickname { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public DateTime dateCreated { get; set; } = DateTime.Now;
    }
}