using System;

namespace Data.SceneContainer
{
    public abstract class BaseSceneContextData
    {
    }
    
    [Serializable]
    public class MainMenuSceneContextData : BaseSceneContextData
    {
        public void Print()
        {
            Console.WriteLine("MainMenuSceneContextData");
        }
    }
}