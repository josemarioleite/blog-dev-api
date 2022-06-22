using System.ComponentModel.DataAnnotations;

namespace blog_api_dev.Models.User
{
    public class UserDTO
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string nickname { get; set; }
    }
}