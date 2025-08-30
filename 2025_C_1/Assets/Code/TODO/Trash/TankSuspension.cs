using UnityEngine;

namespace Code.TODO
{
    public class TankSuspension : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Rigidbody _tankRigidBody;
        [SerializeField] private Transform _tireTransform;
   
        [Header("Suspension Settings")]
        [SerializeField] private float suspensionRestDist = 0.5f; 
        [SerializeField] private float springStrength = 20000f;     // жёсткость пружины
        [SerializeField] private float springDamper = 1500f;        // демпфер
        [SerializeField] private float rayLength = 1.0f;            // длина рейкаста
        [SerializeField] LayerMask _layerMask;
 
        private void FixedUpdate()
        {
            Vector3 springDir = _tireTransform.up;
        
            Vector3 down = - springDir;
        
            Ray tireRay = new Ray(_tireTransform.position, down);
        
            if (Physics.Raycast(tireRay, out RaycastHit  hit, rayLength, _layerMask))
            {
           
                Vector3 tireWorldVel = _tankRigidBody.GetPointVelocity(_tireTransform.position);
            
                float offset = suspensionRestDist - hit.distance;
            
                float vel = Vector3.Dot(springDir, tireWorldVel);
            
                float force = (offset * springStrength) - (vel * springDamper);
            
                _tankRigidBody.AddForceAtPosition(springDir * force, _tireTransform.position, ForceMode.Impulse);
            
            }
        }
    }
}
