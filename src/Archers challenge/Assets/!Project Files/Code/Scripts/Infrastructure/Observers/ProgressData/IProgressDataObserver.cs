namespace Infrastructure.Observers.ProgressData
{
    public interface IProgressDataObserver
    {
        event System.Action<Data.Progress.ProgressData> OnProgressDataUpdated;
        void UpdateProgressData(Data.Progress.ProgressData progressData);
    }
}