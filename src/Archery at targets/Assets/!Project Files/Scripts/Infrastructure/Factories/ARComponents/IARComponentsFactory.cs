using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Factories.ARComponents
{
    public interface IARComponentsFactory
    {
        Task CreateARComponent<T>() where T : Behaviour;
    }
}