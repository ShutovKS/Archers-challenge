namespace Infrastructure.Observers.ProgressData
{
    public interface IProgressDataObserver
    {
        event System.Action<Data.Progress.ProgressData> OnProgressDataUpdated;
        void UpdateProgressData(Data.Progress.ProgressData progressData);
    }

    public class ProgressDataObserver : IProgressDataObserver
    {
        public event System.Action<Data.Progress.ProgressData> OnProgressDataUpdated;

        public void UpdateProgressData(Data.Progress.ProgressData progressData)
        {
            OnProgressDataUpdated?.Invoke(progressData);
        }
    }
}