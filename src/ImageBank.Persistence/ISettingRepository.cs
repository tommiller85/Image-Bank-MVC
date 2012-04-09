using ImageBank.Core;

namespace ImageBank.Persistence
{
    public interface ISettingRepository : IGenericRepository<Setting, int>
    {
        string OriginalImageRoot { get; }
        string MediumImageRoot { get; }
    }
}