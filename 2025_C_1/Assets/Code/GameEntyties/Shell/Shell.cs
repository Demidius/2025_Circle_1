using UnityEngine;
using Zenject;

namespace Code.GameEntyties.Shell
{
    public class Shell : MonoBehaviour
    {
        [Inject]
        public void Construct(Pool pool) => _pool = pool;
        // [Inject]
        // private void Explosion.Pool _explosionPool;
        
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _lifeTime = 5f;

        
        private float _deathAt;
        private bool _despawned;
        private Pool _pool;
        

        private void OnEnable()
        {
            _deathAt = Time.time + _lifeTime;
            _despawned = false;
        }

        private void Update()
        {
            if (!_despawned && Time.time >= _deathAt)
                DespawnOnce();
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
                DespawnOnce();
        }

        private void DespawnOnce()
        {
            if (_despawned) return;
            _despawned = true;
            
            _pool.Despawn(this);
        }

        public class Pool : MonoMemoryPool<Vector3, Quaternion, float, Shell>
        {
            protected override void Reinitialize(Vector3 pos, Quaternion rot, float speed, Shell item)
            {
                var t = item.transform;
                t.SetPositionAndRotation(pos, rot);

                var rb = item._rb;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(t.forward * speed, ForceMode.VelocityChange);
            }

            protected override void OnDespawned(Shell item)
            {
               
                var rb = item._rb;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                item._despawned = false; 
            }
        }
    }
}
