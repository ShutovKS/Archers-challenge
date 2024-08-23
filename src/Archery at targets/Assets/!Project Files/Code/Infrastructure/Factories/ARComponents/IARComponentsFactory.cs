using UnityEngine;

namespace Infrastructure.Factories.ARComponents
{
    public interface IARComponentsFactory
    {
        T Create<T>() where T : Behaviour;
        void Remove<T>() where T : Behaviour;
        T Get<T>() where T : Behaviour;
    }
}