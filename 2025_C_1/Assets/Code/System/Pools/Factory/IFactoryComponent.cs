using UnityEngine;
namespace CodeBase.System.GameSystems.Pools.Factory
{
    public interface IFactoryComponent
    {
        T Create<T>(T prefab) where T : Component;
    }
}