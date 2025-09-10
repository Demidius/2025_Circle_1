using UnityEngine;
using Zenject;
using CodeBase.System.GameSystems.Pools;

namespace Code.GameEntyties.Shell
{
    /// <summary>
    /// Снаряд под кастомный пул. Создаётся через IPoolController.GetPool<Shell>().GetElement()
    /// После получения ОБЯЗАТЕЛЬНО вызвать Init(pos, rot, speed).
    /// </summary>
    public class Shell : MonoBehaviour, IPoolsElement
    {
        [Header("Refs")]
        [SerializeField] private Rigidbody _rb;          // Rigidbody снаряда
        [SerializeField] private GameObject _gfx;        // визуал (опц.)
        [SerializeField] private Collider _col;          // коллайдер (опц.)

        [Header("Life")]
        [SerializeField] private float _lifeTime = 5f;   // время жизни, сек

        private float _deathAt;
        private bool _despawned;

        // DI
        [Inject] private IPoolController _poolController;

        // Кэш слоя Ground
        private static int s_groundLayer = -2;           // -2 = не инициал; -1 = слоя нет

        private void Awake()
        {
            if (s_groundLayer == -2)
            {
                s_groundLayer = LayerMask.NameToLayer("Ground");
                if (s_groundLayer == -1)
                    Debug.LogWarning("[Shell] Layer 'Ground' не найден. Можно использовать тег 'Ground'.");
            }

            if (_rb == null)  _rb  = GetComponent<Rigidbody>();
            if (_col == null) _col = GetComponent<Collider>();
        }

        private void OnEnable() => enabled = true;
        private void OnDisable() => enabled = false;

        /// <summary>Вызывай сразу после получения из пула</summary>
        public void Init(Vector3 pos, Quaternion rot, float speed)
        {
            transform.SetPositionAndRotation(pos, rot);

            _despawned = false;
            _deathAt = Time.time + Mathf.Max(0f, _lifeTime);

            if (_gfx) _gfx.SetActive(true);
            if (_col) _col.enabled = true;

            if (_rb)
            {
                _rb.linearVelocity = transform.forward * speed; // предпочтительно linearVelocity
                _rb.angularVelocity = Vector3.zero;
            }
        }

        private void Update()
        {
            if (!_despawned && Time.time >= _deathAt)
                DespawnOnce();
        }

        private void OnCollisionEnter(Collision col)
        {
            if (_despawned) return;

            bool hitGround =
                (s_groundLayer != -1 && col.gameObject.layer == s_groundLayer) ||
                col.gameObject.CompareTag("Ground");

            if (hitGround)
            {
                // Спавним взрыв через твой пул
                var explosion = _poolController.GetPool<Explosion>().GetElement();
                explosion.Init(transform.position, transform.rotation, 0f); // speed тут не нужен
                DespawnOnce();
            }
        }

        private void DespawnOnce()
        {
            if (_despawned) return;
            _despawned = true;

            Deactivate(); // сбросим состояние
            _poolController.ReturnToPool(this); // вернём в пул (он выключит объект)
        }

        // ===== IPoolsElement =====
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
