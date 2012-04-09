using ImageBank.Core;
using ImageBank.Persistence;
using ImageBank.Services.Account;
using Moq;
using NUnit.Framework;

namespace ImageBank.Tests.ServiceTests
{
    [TestFixture]
    public class AccountServiceTests
    {
        [Test]
        public void Authenticate_WhenCredentialsAreCorrect_ShouldReturnTrue()
        {
            var accountService = GetAccountService();

            bool result = accountService.Authenticate("testuser", "correctpassword");

            Assert.IsTrue(result);
        }

        [Test]
        public void Authenticate_WhenCredentialsAreIncorrect_ShouldReturnFalse()
        {
            var accountService = GetAccountService();

            bool result = accountService.Authenticate("testuser", "incorrectpassword");

            Assert.IsFalse(result);
        }

        [Test]
        public void Authenticate_WhenUserDoesntExist_ShouldReturnFalse()
        {
            var accountService = GetAccountService();

            bool result = accountService.Authenticate("unknownuser", "password");

            Assert.IsFalse(result);
        }

        private AccountService GetAccountService()
        {
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(x => x.Get("testuser")).Returns(new User
                                                                         {
                                                                             Username = "testuser",
                                                                             Password = "correctpassword"
                                                                         });
            var accountService = new AccountService(mockUserRepository.Object);

            return accountService;
        }
    }
}