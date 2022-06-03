using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace blog_api_dev.Models.User
{
    [BsonIgnoreExtraElements]
    public class User
    {
        public ObjectId id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string nickname { get; set; }
    }
}