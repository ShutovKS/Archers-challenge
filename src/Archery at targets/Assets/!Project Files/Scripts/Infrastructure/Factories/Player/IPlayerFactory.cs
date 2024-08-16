using Infrastructure.Factories.GameObjects;
using UnityEngine;

namespace Infrastructure.Factories.Player
{
    public interface IPlayerFactory
    {
        GameObject CreatePlayer(Vector3 position, Quaternion rotation);
    }

    public class PlayerFactory : IPlayerFactory
    {
        private readonly IGameObjectFactory _gameObjectFactory;

        public PlayerFactory(IGameObjectFactory gameObjectFactory)
        {
            _gameObjectFactory = gameObjectFactory;
        }

        public GameObject CreatePlayer(Vector3 position, Quaternion rotation)
        {
            return null;
        }
    }
}