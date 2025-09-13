using Unity.VisualScripting;
using UnityEngine;
namespace Code.GameEntyties.Target
{
    public class DamageTaker : MonoBehaviour, IDamageTaker
    {
        private IEntityBase _entity;
     
        void Awake() => _entity = GetComponentInParent<IEntityBase>();

        public void ApplyDamage(DamageInfo info)
        {
            _entity?.TakeDamage(info);
        }
    }
}
