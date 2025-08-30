using UnityEngine;
namespace Code.TODO
{
    public class TrackToWheel : MonoBehaviour
    {
        [SerializeField] private GameObject _trackBone;
        
        private void Update()
        {
            _trackBone.transform.position = transform.position + new Vector3(0, -0.16f, 0);
        }
        
        
    }
}
