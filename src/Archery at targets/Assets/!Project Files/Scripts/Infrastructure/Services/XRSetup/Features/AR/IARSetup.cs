namespace Infrastructure.Services.XRSetup.Features.AR
{
    public interface IARSetup
    {
        ARFeature Feature { get; }
        void Enable(bool enable);
        bool IsEnabled();
    }
}