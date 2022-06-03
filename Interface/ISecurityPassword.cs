using blog_api_dev.Models.User;

namespace blog_api_dev.Interface
{
    public interface ISecurityPassword
    {
        bool ComparePassword(UserAuth userAuth);
        bool VerifyPassword(UserAuth userAuth, string password);
        string EncryptPassword(string password);
    }
}