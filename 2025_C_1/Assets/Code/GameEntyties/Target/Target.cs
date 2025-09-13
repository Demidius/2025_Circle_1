using Code.GameEntyties.Shell;
using CodeBase.System.GameSystems.Pools;
using UnityEngine;
using Zenject;

namespace Code.GameEntyties.Target
{
    public class Target : MonoBehaviour, IEntityBase, IPoolsElement
    {
        [Inject] private IPoolController _poolController;

        private void OnEnable()
        {
            Hp = 1;
        }

        public int Hp
        {
            get;
            set;
        }
        
        public void TakeDamage(DamageInfo _info)
        {
            Debug.Log(gameObject.name + " taking damage" + Hp + " damage");
            Hp -= _info.Amount;
            if (Hp <= 0)
                Deactivate();
        }
        public void Deactivate()
        {
            Debug.Log(gameObject.name + " deactivated");
            
            var explosion = _poolController.GetPool<ExplosionBig>().GetElement();
            explosion.Init(transform.position, transform.rotation, 0f);
            _poolController.ReturnToPool(this);
        }
    }
}
