using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using blog_api_dev.Models;
using blog_api_dev.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace blog_api_dev.Utils
{
  public class SecurityPassword
  {
    public async Task<string> CreateTokenJWT(User user, JwtSettings jwtSettings) {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor {
          Subject = new ClaimsIdentity(new [] {
              new Claim("id", user.id.ToString())
          }),
          Expires = DateTime.UtcNow.AddHours(12),
          SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }

    public async Task<int> ReturnIdUserToken(HttpContext context) {
      var identidade = context.User.Identity as ClaimsIdentity;
      if(identidade != null) {
          IEnumerable<Claim> claims = identidade.Claims;
          int idusuario = Int32.Parse(identidade.FindFirst("id").Value);
          return idusuario;
      } else {
          return 0;
      }
    }
  }
}