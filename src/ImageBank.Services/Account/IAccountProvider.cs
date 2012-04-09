using ImageBank.Core;

namespace ImageBank.Services.Account
{
    public interface IAccountProvider
    {
        RegistrationResult Register(User user);
        bool Authenticate(string username, string password);
        void SetAuthCookie(string username, bool createPersistentCookie);
    }
}