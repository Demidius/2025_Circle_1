using Code.GameEntyties.Player.Turret;
using UnityEngine;
using Zenject;
using CodeBase.System.GameSystems.Pools;

namespace Code.GameEntyties.Shell
{
    public class Gun : MonoBehaviour
    {
        [Header("Shoot")]
        [SerializeField] private Transform _muzzle;
        [SerializeField] private float _muzzleSpeed = 30f;
        [SerializeField] private float _fireRate = 10f; // выстрелов в сек
        [SerializeField] private ShootingFireEffect _shootingFire;
        
        private float _nextShotAt;

        [Inject] private IPoolController _poolController;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                TryFire();
        }

        private void TryFire()
        {
            if (Time.time < _nextShotAt) return;
            _nextShotAt = Time.time + 1f / Mathf.Max(0.001f, _fireRate);

            var pos = _muzzle ? _muzzle.position : transform.position;
            var rot = _muzzle ? _muzzle.rotation : transform.rotation;

            _shootingFire.gameObject.SetActive(true);
            
            // Берём снаряд из пула и инициализируем
            var shell = _poolController.GetPool<Shell>().GetElement();
            shell.Init(pos, rot, _muzzleSpeed);
        }
    }
}
