using System.Linq;
using ImageBank.Core;

namespace ImageBank.Persistence
{
    public class SettingRepository : GenericRepository<ImageBankContext, Setting, int>, ISettingRepository
    {
        public SettingRepository(ImageBankContext ctx)
            : base(ctx)
        {
        }

        public string OriginalImageRoot
        {
            get { return Context.Settings.Single(s => s.Key == "OriginalImageRoot").Value; }
        }

        public string MediumImageRoot
        {
            get { return Context.Settings.Single(s => s.Key == "MediumImageRoot").Value; }
        }
    }
}