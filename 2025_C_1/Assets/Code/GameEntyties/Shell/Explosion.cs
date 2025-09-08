using System;
using UnityEngine;
using Zenject;
namespace Code.GameEntyties.Shell
{
    public class Explosion : MonoBehaviour
    {
        private IMemoryPool _pool;
        // [Inject]
        // public void Construct(Pool pool) => _pool = pool;
        
        public void OnSpawned(Vector3 pos, Quaternion rot, IMemoryPool pool)
        {
           _pool = pool;
           transform.position = pos;
           transform.rotation = rot;
           
           
        }
        public void OnDespawned()
        {
           
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }
        
        
        public class Pool : MonoMemoryPool<Vector3,Quaternion,Explosion>
        {
            
            
          
        }
    }
}
