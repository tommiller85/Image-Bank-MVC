using System;
using System.Web.Security;
using ImageBank.Core;
using ImageBank.Persistence;

namespace ImageBank.Services.Account
{
    public class AccountProvider : IAccountProvider
    {
        private readonly IUserRepository _userRepository;

        public AccountProvider(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public RegistrationResult Register(User user)
        {
            throw new NotImplementedException();
        }

        public bool Authenticate(string username, string password)
        {
            var userToAuthenticate = _userRepository.Get(username);
            if (userToAuthenticate != null && userToAuthenticate.Password == password)
                return true;
            return false;
        }

        public void SetAuthCookie(string username, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(username, createPersistentCookie);
        }
    }
}