using ImageBank.Core;

namespace ImageBank.Persistence
{
    public class UserRepository : GenericRepository<ImageBankContext, User, string>, IUserRepository
    {
        public UserRepository(ImageBankContext ctx)
            : base(ctx)
        {
        }
    }
}