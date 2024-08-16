namespace Infrastructure.Services.XRSetup.AR
{
    public interface IARSetup
    {
        ARFeature Feature { get; }
        void Enable(bool enable);
        bool IsEnabled();
    }
}