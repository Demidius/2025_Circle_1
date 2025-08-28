using Code.GameEntyties.Player;
using UnityEngine;
namespace Code.Level
{
    public class LevelFloat : MonoBehaviour
    {
        private PlayerTag _playerTag;
        
        private void Start()
        {
            _playerTag = FindObjectOfType<PlayerTag>();
            
            _playerTag.gameObject.transform.position = new Vector3(0, 0.5f, 0);
        }
        
        
        
        
    }
}
