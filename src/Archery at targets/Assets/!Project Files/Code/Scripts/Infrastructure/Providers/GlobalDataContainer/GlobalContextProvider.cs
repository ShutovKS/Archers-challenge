using Data.Contexts.Global;

namespace Infrastructure.Providers.GlobalDataContainer
{
    public class GlobalContextProvider : IGlobalContextProvider
    {
        public GlobalContextData GlobalContext { get; } = new();
    }
}