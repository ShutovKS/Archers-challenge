using System;

namespace Infrastructure.Services.InteractorSetup
{
    [Flags]
    public enum InteractorType
    {
        None = 0,
        Direct = 1 << 0,
        Gaze = 1 << 1,
        NearFar = 1 << 2,
        Poke = 1 << 3,
        Ray = 1 << 4,
        Socket = 1 << 5,
    }
}