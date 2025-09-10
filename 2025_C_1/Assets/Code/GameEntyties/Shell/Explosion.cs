using UnityEngine;
using Zenject;
using CodeBase.System.GameSystems.Pools;
using UnityEngine.VFX;

namespace Code.GameEntyties.Shell
{
    public class Explosion : MonoBehaviour, IPoolsElement
    {
        [Header("VFX")]
        [SerializeField] private VFXRenderer _vfx;
        [SerializeField] private float _lifeTime = 0.6f;

        [Header("Light")]
        [SerializeField] private Light _light;
        [SerializeField] private float _maxIntensity = 800f; // стартовая яркость
        [SerializeField] private AnimationCurve _intensityOverLife =
            new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f)); // 1→0

        [Inject] private IPoolController _poolController;

        private float _startAt;
        private float _deathAt;
        private bool _despawned;


        private void OnEnable()  => enabled = true;
        private void OnDisable() => enabled = false;

        /// <summary>speed игнорируется — для унификации сигнатуры</summary>
        public void Init(Vector3 pos, Quaternion rot, float _)
        {
            transform.SetPositionAndRotation(pos, rot);

            _despawned = false;
            _startAt = Time.time;
            _deathAt = _startAt + Mathf.Max(0f, _lifeTime);
            
            if (_light)
            {
                _light.enabled = true;
                _light.intensity = _maxIntensity;
            }
        }


        private void Update()
        {
            float frac = Mathf.InverseLerp(_startAt, _deathAt, Time.time);
            
            if (_light)
            {
                float k = _intensityOverLife.Evaluate(frac); 
                _light.intensity = _maxIntensity * k;       
            }
            
            if (!_despawned && Time.time >= _deathAt)
                DespawnOnce();
        }

        private void DespawnOnce()
        {
            if (_despawned) return;
            _despawned = true;
            _poolController.ReturnToPool(this);
        }
        public void Deactivate() => DespawnOnce();
    }
}
