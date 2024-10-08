#region

using Data.Progress;

#endregion

namespace Infrastructure.Services.Progress
{
    public delegate void ProgressDataChangedEventHandler(ProgressData progressData);

    public interface IProgressService
    {
        void Set(ProgressData progressData);
        ProgressData Get();
        void Reset();
    }
}