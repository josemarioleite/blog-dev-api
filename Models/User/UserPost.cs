using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace blog_api_dev.Models.User
{
    public class UserPost
    {
        [Required(ErrorMessage = "Preencha o seu Nome")]
        [MaxLength(30)]
        public string name { get; set; }
        [Required(ErrorMessage = "Preencha seu Nickname")]
        [MaxLength(30)]
        public string nickname { get; set; }
        public string password_post { get; set; }
        [JsonIgnore]
        public byte[] password { get; set; }
        [JsonIgnore]
        public byte[] password_key { get; set; }
    }
}