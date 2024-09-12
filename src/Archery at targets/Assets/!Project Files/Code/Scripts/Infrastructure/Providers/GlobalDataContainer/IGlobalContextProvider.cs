using Data.Contexts.Global;

namespace Infrastructure.Providers.GlobalDataContainer
{
    public interface IGlobalContextProvider
    {
        GlobalContextData GlobalContext { get; }
    }
}