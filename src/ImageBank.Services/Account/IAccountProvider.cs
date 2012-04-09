using ImageBank.Core;

namespace ImageBank.Services.Account
{
    public interface IAccountProvider
    {
        RegistrationResult Register(User user);
    }
}