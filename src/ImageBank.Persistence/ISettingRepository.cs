using ImageBank.Core;

namespace ImageBank.Persistence
{
    public interface ISettingRepository : IGenericRepository<Setting>
    {
        string OriginalImageRoot { get; }
        string MediumImageRoot { get; }
    }
}