using System.Collections.Generic;

namespace Zenject
{
    public interface ICompositeInstaller<out T> : IInstaller where T : IInstaller
    {
        IReadOnlyList<T> LeafInstallers { get; }
    }
}