#region

using Data.Progress;
using Infrastructure.Observers.ProgressData;
using Infrastructure.Services.DataStorage;
using JetBrains.Annotations;

#endregion

namespace Infrastructure.Services.Progress
{
    [UsedImplicitly]
    public class ProgressService : IProgressService
    {
        private const string PROGRESS_DATA_KEY = "ProgressData";
        private readonly IDataStorageService _dataStorageService;
        private readonly IProgressDataObserver _progressDataObserver;
        private ProgressData _progressData;

        public ProgressService(IDataStorageService dataStorageService, IProgressDataObserver progressDataObserver)
        {
            _dataStorageService = dataStorageService;
            _progressDataObserver = progressDataObserver;
        }

        public void Set(ProgressData progressData)
        {
            _progressData = progressData;

            _dataStorageService.Save(PROGRESS_DATA_KEY, progressData);

            _progressDataObserver.UpdateProgressData(progressData);
        }

        public ProgressData Get()
        {
            if (_progressData != null)
            {
                return _progressData;
            }

            if (_dataStorageService.Exists(PROGRESS_DATA_KEY))
            {
                _progressData = _dataStorageService.Load<ProgressData>(PROGRESS_DATA_KEY);
            }
            else
            {
                _progressData = ProgressData.CreateDefault();
            }

            return _progressData;
        }

        public void Reset()
        {
            _progressData = ProgressData.CreateDefault();

            _dataStorageService.Delete(PROGRESS_DATA_KEY);

            _progressDataObserver.UpdateProgressData(_progressData);
        }
    }
}