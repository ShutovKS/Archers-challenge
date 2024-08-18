using System.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Factories.ARComponents
{
    public interface IARComponentsFactory
    {
        Task<T> CreateARComponent<T>() where T : Behaviour;
        void RemoveARComponent<T>() where T : Behaviour;
    }
}