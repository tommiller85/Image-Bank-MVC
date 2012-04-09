using ImageBank.Core;
using ImageBank.Persistence;
using ImageBank.Services.Account;
using Moq;
using NUnit.Framework;

namespace ImageBank.Tests.ServiceTests
{
    [TestFixture]
    public class AccountProviderTests
    {
        [Test]
        public void Authenticate_WhenCredentialsAreCorrect_ShouldReturnTrue()
        {
            var accountProvider = GetAccountProvider();

            bool result = accountProvider.Authenticate("testuser", "correctpassword");

            Assert.IsTrue(result);
        }

        [Test]
        public void Authenticate_WhenCredentialsAreIncorrect_ShouldReturnFalse()
        {
            var accountProvider = GetAccountProvider();

            bool result = accountProvider.Authenticate("testuser", "incorrectpassword");

            Assert.IsFalse(result);
        }

        [Test]
        public void Authenticate_WhenUserDoesntExist_ShouldReturnFalse()
        {
            var accountProvider = GetAccountProvider();

            bool result = accountProvider.Authenticate("unknownuser", "password");

            Assert.IsFalse(result);
        }

        private AccountProvider GetAccountProvider()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.Get("testuser")).Returns(new User
                                                                         {
                                                                             Username = "testuser",
                                                                             Password = "correctpassword"
                                                                         });
            var accountProvider = new AccountProvider(mockUserRepository.Object);

            return accountProvider;
        }
    }
}