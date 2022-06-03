using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace blog_api_dev.Models.User
{
  public class UserAuth
  {
    [Required]
    public string nickname { get; set; }
    [Required]
    public string password { get; set; }
    [Required]
    [BsonIgnore]
    public string confirmPassword { get; set; }
  }
}