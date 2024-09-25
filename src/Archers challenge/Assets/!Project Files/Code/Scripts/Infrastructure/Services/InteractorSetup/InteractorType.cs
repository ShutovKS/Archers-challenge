#region

using System;

#endregion

namespace Infrastructure.Services.InteractorSetup
{
    [Flags]
    public enum InteractorType
    {
        Nothing = 0,

        Direct = 1 << 0,
        Poke = 1 << 2,
        Ray = 1 << 3,

        Gaze = 1 << 9,

        All = Direct | Poke | Ray | Gaze
    }

    public enum HandType
    {
        None,
        Left,
        Right,
    }

    [Serializable]
    public class InteractorData
    {
        public InteractorData()
        {
        }

        public InteractorData(HandType handType, InteractorType interactorType)
        {
            HandType = handType;
            InteractorType = interactorType;
        }

        public HandType HandType { get; set; }
        public InteractorType InteractorType { get; set; }
    }
}