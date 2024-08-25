using System;

namespace Infrastructure.Services.InteractorSetup
{
    [Flags]
    public enum InteractorType
    {
        Nothing = 0,

        Direct = 1 << 0,
        NearFar = 1 << 1,
        Poke = 1 << 2,
        Ray = 1 << 3,
        
        Gaze = 1 << 9,
        
        All = Direct | NearFar | Poke | Ray | Gaze
    }
}