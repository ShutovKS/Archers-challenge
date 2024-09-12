using JetBrains.Annotations;

namespace Infrastructure.Observers.ProgressData
{
    public class ProgressDataObserver : IProgressDataObserver
    {
        public event System.Action<Data.Progress.ProgressData> OnProgressDataUpdated;

        public void UpdateProgressData(Data.Progress.ProgressData progressData)
        {
            OnProgressDataUpdated?.Invoke(progressData);
        }
    }
}