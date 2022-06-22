using System.ComponentModel.DataAnnotations;

namespace blog_api_dev.Models.User
{
  public class UserAuth
  {
    [Required(ErrorMessage = "Preencha o seu nickname")]
    public string nickname { get; set; }
    [Required(ErrorMessage = "Preencha sua senha")]
    public string password { get; set; }
  }
}