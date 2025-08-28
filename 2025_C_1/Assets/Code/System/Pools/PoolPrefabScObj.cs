using System.Collections.Generic;
using UnityEngine;
namespace CodeBase.System.GameSystems.Pools
{
    
    [CreateAssetMenu(fileName = "PoolPrefabScObj", menuName = "ScObj/PoolPrefabScObj")]
    
    public class PoolPrefabScObj: ScriptableObject
    {
        public List<GameObject> PoolPrefabs;
    }
}