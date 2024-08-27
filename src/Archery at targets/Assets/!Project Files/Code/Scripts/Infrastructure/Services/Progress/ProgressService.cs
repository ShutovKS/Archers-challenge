using Data.Progress;
using Infrastructure.Services.DataStorage;

namespace Infrastructure.Services.Progress
{
    public class ProgressService : IProgressService
    {
        public event ProgressDataChangedEventHandler ProgressDataChanged;

        private const string PROGRESS_DATA_KEY = "ProgressData";
        private readonly IDataStorageService _dataStorageService;
        private ProgressData _progressData;

        public ProgressService(IDataStorageService dataStorageService)
        {
            _dataStorageService = dataStorageService;
        }

        public void Set(ProgressData progressData)
        {
            _progressData = progressData;

            _dataStorageService.Save(PROGRESS_DATA_KEY, progressData);

            ProgressDataChanged?.Invoke(_progressData);
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
            
            ProgressDataChanged?.Invoke(_progressData);
        }
        
        public void SetCurrentWeapon(string weaponId)
        {
            var progressData = Get();
            progressData.currentWeaponId = weaponId;
            Set(progressData);
        }
        
        public string GetCurrentWeaponId()
        {
            return Get().currentWeaponId;
        }
    }
}