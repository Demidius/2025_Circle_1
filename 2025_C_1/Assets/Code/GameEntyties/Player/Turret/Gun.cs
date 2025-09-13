using Code.GameEntyties.Player.Turret;
using UnityEngine;
using Zenject;
using CodeBase.System.GameSystems.Pools;
using UnityEngine.VFX;

namespace Code.GameEntyties.Shell
{
    public class Gun : AudioSoursMono
    {
        [Header("Components")]
        [SerializeField] private Transform _muzzle;
        [SerializeField] private VisualEffect _shootingFire;
        [SerializeField] private Rigidbody _playerRb;
        
        [Header("Parameters")]
        [SerializeField] private float _muzzleSpeed = 30f;
        [SerializeField] private float _fireRate = 10f; // выстрелов в сек
        [SerializeField] private float _powerThrowback = 10;

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

            if (_shootingFire != null)
                _shootingFire.SendEvent("OnPlay");
            
            _playerRb.AddForceAtPosition(-_muzzle.transform.forward * _powerThrowback, _muzzle.position, ForceMode.Force);
            
            _audioManager.PlaySound(_audioTracksBase.GunShoot);
            
            // Берём снаряд из пула и инициализируем
            var shell = _poolController.GetPool<Shell>().GetElement();
            shell.Init(pos, rot, _muzzleSpeed);
        }
    }
}
