using UnityEngine;
using Zenject;

namespace Code.GameEntyties.Player
{
    public class Gun : MonoBehaviour
    {
        [Inject] private Code.GameEntyties.Shell.Shell.Pool _shellPool;

        [SerializeField] private Transform _muzzle; // точка выстрела
        [SerializeField] private float _fireSpeed = 200f;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var pos = _muzzle ? _muzzle.position : transform.position;
                var rot = _muzzle ? _muzzle.rotation : transform.rotation;

                _shellPool.Spawn(pos , rot, _fireSpeed);
            }
        }
    }
}
