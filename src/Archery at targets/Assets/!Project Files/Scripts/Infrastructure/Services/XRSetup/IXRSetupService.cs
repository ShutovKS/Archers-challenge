using System.Threading.Tasks;

namespace Infrastructure.Services.XRSetup
{
    public interface IXRSetupService
    {
        Task SetXRMode(XRMode mode);
    }
}