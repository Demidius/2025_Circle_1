using System;
using System.Collections.Generic;
using CodeBase.System.GameSystems.Pools.Factory;
using UnityEngine;
using Zenject;
namespace CodeBase.System.GameSystems.Pools
{
    public interface IPoolController
    {
        PoolComponent<T> GetPool<T>() where T : MonoBehaviour, IPoolsElement;
        void             ReturnToPool<T>(T element) where T : MonoBehaviour, IPoolsElement;
    }

    public class PoolController : MonoBehaviour, IPoolController
    {
        [SerializeField] private PoolPrefabScObj poolPrefabScObj;
        private int startPoolSize = 1;

        private Dictionary<Type, object> _poolsDictionary;
        private IFactoryComponent _factoryComponent;
    

        [Inject]
        public void Construct(IFactoryComponent factoryComponent)
        {
            _factoryComponent = factoryComponent;
        }

        private void Awake()
        {
            _poolsDictionary = new Dictionary<Type, object>();
        }

        private void Start()
        {
            if (poolPrefabScObj?.PoolPrefabs == null || poolPrefabScObj.PoolPrefabs.Count == 0)
            {
                Debug.LogError("No pool prefabs found.");
                return;
            }

            foreach (var poolElement in poolPrefabScObj.PoolPrefabs)
            {
                if (poolElement == null)
                {
                    Debug.LogWarning("Skipping null pool element.");
                    continue;
                }

                RegisterPool(poolElement);
            }
        }

        private void RegisterPool(GameObject poolElement)
        {
            var element = poolElement.GetComponent<IPoolsElement>();
            if (element == null) return;

            var elementType = element.GetType();
            if (_poolsDictionary.ContainsKey(elementType)) return;

            _poolsDictionary[elementType] = CreatePool(poolElement, elementType);
        }

        private object CreatePool(GameObject poolElement, Type elementType)
        {
            var prefab = poolElement.GetComponent(elementType) as MonoBehaviour;
            if (prefab == null)
            {
                Debug.LogError($"Prefab of type {elementType} not found on the pool element.");
                return null;
            }

            var poolType = typeof(PoolComponent<>).MakeGenericType(elementType);
            try
            {
                return Activator.CreateInstance(poolType, prefab, startPoolSize, transform, _factoryComponent);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating pool for {elementType}: {ex.Message}");
                return null;
            }
        }

        public PoolComponent<T> GetPool<T>() where T : MonoBehaviour, IPoolsElement
        {
            var type = typeof(T);

            if (_poolsDictionary.TryGetValue(type, out var pool))
            {
                if (pool is PoolComponent<T> typedPool)
                {
                    return typedPool;
                }
                else
                {
                    Debug.LogError($"Pool for type {type} is not of the expected type.");
                    return null;
                }
            }

            Debug.LogError($"Pool for type {type} not found.");
            return null;
        }

        public void ReturnToPool<T>(T element) where T : MonoBehaviour, IPoolsElement
        {
            var pool = GetPool<T>();
            pool?.ReturnToPool(element);
        }
    }
}

