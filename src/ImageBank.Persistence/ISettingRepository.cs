using ImageBank.Core;

namespace ImageBank.Persistence
{
    public interface ISettingRepository : IGenericRepository<Setting>
    {
        string ImageRoot { get; }
    }
}