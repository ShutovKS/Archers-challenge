using System;

namespace Zenject
{
    [NoReflectionBaking]
    public class AsyncFromBinderBase : ScopeConcreteIdArgConditionCopyNonLazyBinder
    {
        public AsyncFromBinderBase(DiContainer bindContainer, Type contractType, BindInfo bindInfo)
            : base(bindInfo)
        {
            BindContainer = bindContainer;
            ContractType = contractType;
        }
        
        internal DiContainer BindContainer
        {
            get; private set;
        }
        
        protected Type ContractType
        {
            get; private set;
        }

    }
}