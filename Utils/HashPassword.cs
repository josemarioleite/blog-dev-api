using blog_api_dev.Models.User;

namespace blog_api_dev.Utils
{
  public class HashPassword
  {
    public void HasheiaSenha(UserPost user) {
      if (user.password_post != null && !string.IsNullOrWhiteSpace(user.password_post)) {
        using (var hmac = new System.Security.Cryptography.HMACSHA512()){
          user.password = hmac.Key;
          user.password_key = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.password_post));
        }
      }
    }

    public bool VerifyPasswordHash(User user, string password) {
      if (password != null && !string.IsNullOrWhiteSpace(password)) {
        if (user.password.Length == 128 && user.password_key.Length == 64) {
          using (var hmac = new System.Security.Cryptography.HMACSHA512(user.password)) {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++) {
              if (computedHash[i] != user.password_key[i]) {
                return false;
              }
            }
            return true;
          }
        } else {
          return false;
        }
      } else {
        return false;
      }
    }
  }
}