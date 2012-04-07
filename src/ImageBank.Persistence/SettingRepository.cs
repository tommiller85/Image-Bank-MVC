using System.Linq;
using ImageBank.Core;

namespace ImageBank.Persistence
{
    public class SettingRepository : GenericRepository<ImageBankContext, Setting>, ISettingRepository
    {
        public SettingRepository(ImageBankContext ctx)
            : base(ctx)
        {
        }

        public string ImageRoot
        {
            get { return Context.Settings.Single(s => s.Key == "ImageRoot").Value; }
        }
    }
}