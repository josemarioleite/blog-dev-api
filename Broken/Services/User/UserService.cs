using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using blog_api_dev.Models;
using blog_api_dev.Models.User;
using blog_api_dev.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog_api_dev.Broken.Services.User
{
    public class UserService
    {
        private readonly DbContextDatabase _database;
        private readonly IMapper _imapper;

        public UserService(DbContextDatabase database, IMapper imapper = null, JwtSettings jwtSettings = null)
        {
            _database = database;
            _imapper = imapper;
        }

        public async Task<JsonResult> ListUser(int idUser)
        {
            var user = await _database.User.AsNoTracking().FirstOrDefaultAsync(obj => obj.id.Equals(idUser));
            var filter = new UserDTO
            {
                id = user.id,
                name = user.name,
                nickname = user.nickname
            };

            return new JsonResult(filter);
        }

        public async Task<ActionResult> NewUser(UserPost user)
        {
            if (await _database.User.FirstOrDefaultAsync(u => u.nickname.ToLower() == user.nickname.ToLower()) != null) {
                return TypeUtils.ReturnTypeResponseHTTP(false, null, $"O Nickname {user.nickname} já está cadastrado no sistema");
            }
            
            HashPassword hash = new HashPassword();
            hash.HasheiaSenha(user);
            var newUser =_imapper.Map<Models.User.User>(user);
            newUser.password = user.password;
            newUser.password_key = user.password_key;
            await _database.User.AddAsync(newUser);
            try {
                await _database.SaveChangesAsync();
                return TypeUtils.ReturnTypeResponseHTTP(true);
            } catch (Exception ex) {
                return TypeUtils.ReturnTypeResponseHTTP(false, ex);
            }
        }

        public async Task<ActionResult> AuthUser(UserAuth userAuth, JwtSettings jwtSettings)
        {
            Models.User.User user;
            try
            {
                user = await _database.User.FirstOrDefaultAsync(u => u.nickname.Equals(userAuth.nickname));
            } catch (Exception ex)
            {
                return TypeUtils.ReturnTypeResponseHTTP(false, null, $"O Nickname {userAuth.nickname} não existe");
            }
            
            if(user == null)
            {
                ///Usuário não encontrados
                return TypeUtils.ReturnTypeResponseHTTP(false, null, "Usuário não encontrado");
            } else {
                //Verifica se a senha esta correta
                HashPassword hash = new HashPassword();
                if (hash.VerifyPasswordHash(user, userAuth.password))
                {
                    var token = await new SecurityPassword().CreateTokenJWT(user, jwtSettings);
                    return new JsonResult(new {
                        code = 200,
                        status = true,
                        token = token,
                        msg = "Login feito com sucesso!!!"
                    });
                } else 
                {
                    return TypeUtils.ReturnTypeResponseHTTP(false, null, "A senha digitada está incorreta!");
                }
            }
        }
    }
}