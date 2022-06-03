using blog_api_dev.Interface;
using blog_api_dev.Models.User;

namespace blog_api_dev.Utils
{
  public class SecurityPassword : ISecurityPassword
  {
    public bool ComparePassword(UserAuth userAuth)
    {
      return userAuth.password.Trim().Equals(userAuth.confirmPassword.Trim());
    }

    public string EncryptPassword(string password)
    {
      return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(UserAuth userAuth, string password)
    {
      return BCrypt.Net.BCrypt.Verify(password, userAuth.password);
    }
  }
}