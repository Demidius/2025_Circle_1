using UnityEngine;
namespace CodeBase.System.GameSystems.Pools.Factory
{
    public interface IFactory<T> 
    {
        T Create(Vector2 position);
    }
}