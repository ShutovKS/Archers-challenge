using Data.Progress;

namespace Infrastructure.Services.Progress
{
    public delegate void ProgressDataChangedEventHandler(ProgressData progressData);

    public interface IProgressService
    {
        event ProgressDataChangedEventHandler ProgressDataChanged;
        void Set(ProgressData progressData);
        ProgressData Get();
        void Reset();
        
        void SetCurrentWeapon(string weaponId);
        string GetCurrentWeaponId();
    }
}