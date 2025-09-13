
using UnityEngine;
namespace Code.GameEntyties.Target
{

    public interface IDamageTaker
    {
        void ApplyDamage(DamageInfo info);
    }
    
    public struct DamageInfo
    {
        public int Amount;
        public Vector3 Point;
        public Vector3 Normal;
        public Vector3 Direction;

        public DamageInfo(int amount, Vector3 point, Vector3 normal, Vector3 dir)
        {
            Amount = amount; Point = point; Normal = normal; Direction = dir;
        }
    }
}
