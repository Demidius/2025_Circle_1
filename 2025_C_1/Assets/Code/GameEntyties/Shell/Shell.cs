using Code.GameEntyties.Target;
using UnityEngine;
using Zenject;
using CodeBase.System.GameSystems.Pools;

namespace Code.GameEntyties.Shell
{
    public class Shell : MonoBehaviour, IPoolsElement
    {
        [Header("Refs")]
        [SerializeField] Rigidbody _rb;
        [SerializeField] GameObject _gfx;
        [SerializeField] Collider _col;

        [Header("Life")]
        [SerializeField] float _lifeTime = 5f;
        [SerializeField] int _damage = 1;

        [Inject] IPoolController _poolController;

        float _deathAt;
        bool _despawned;

        static int s_groundLayer = -2;
        static int s_targetLayer = -2;

        void Awake()
        {
            if (s_groundLayer == -2) s_groundLayer = LayerMask.NameToLayer("Ground");
            if (s_targetLayer == -2) s_targetLayer = LayerMask.NameToLayer("Target");

            if (!_rb)  _rb  = GetComponent<Rigidbody>();
            if (!_col) _col = GetComponent<Collider>();
        }

        void OnEnable()  => enabled = true;
        void OnDisable() => enabled = false;

        public void Init(Vector3 pos, Quaternion rot, float speed)
        {
            transform.SetPositionAndRotation(pos, rot);
            _despawned = false;
            _deathAt = Time.time + Mathf.Max(0f, _lifeTime);

            if (_gfx) _gfx.SetActive(true);
            if (_col) _col.enabled = true;

            if (_rb)
            {
                _rb.linearVelocity = transform.forward * speed;
                _rb.angularVelocity = Vector3.zero;
                _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            }
        }

        void Update()
        {
            if (!_despawned && Time.time >= _deathAt)
                DespawnOnce();
        }

        void OnCollisionEnter(Collision col)
        {
            if (_despawned) return;

            // 1) Попробуем нанести урон
            var taker = col.collider.GetComponent<IDamageTaker>() 
                     ?? col.collider.GetComponentInParent<IDamageTaker>();
            if (taker != null)
            {
                var cp = col.GetContact(0);
                var info = new DamageInfo(
                    amount: _damage,
                    point: cp.point,
                    normal: cp.normal,
                    dir: _rb ? _rb.linearVelocity.normalized : transform.forward
                );
                taker.ApplyDamage(info);
            }

            // 2) Взрыв/деспавн при любом валидном попадании в мир/цель
            int layer = col.gameObject.layer;
            bool hitSurface =
                (s_groundLayer != -1 && layer == s_groundLayer) ||
                (s_targetLayer != -1 && layer == s_targetLayer) ||
                taker != null; // если был урон — тоже деспавним

            if (hitSurface)
            {
                var explosion = _poolController.GetPool<Explosion>().GetElement();
                explosion.Init(transform.position, transform.rotation, 0f);
                DespawnOnce();
            }
        }

        void DespawnOnce()
        {
            if (_despawned) return;
            _despawned = true;
            Deactivate();
            _poolController.ReturnToPool(this);
        }

        public void Deactivate()
        {
            if (_col) _col.enabled = false;
            if (_gfx) _gfx.SetActive(false);

            if (_rb)
            {
                _rb.linearVelocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
