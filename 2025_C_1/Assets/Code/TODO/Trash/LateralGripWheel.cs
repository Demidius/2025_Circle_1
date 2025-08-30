using UnityEngine;

namespace Code.TODO
{
    public class LateralGripWheel : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] Rigidbody carRb;
        Transform tire;

        [Header("Grip")]
        [SerializeField, Range(0f,1f)] float grip = 1f; // 0—нет сцепления, 1—полное
        [SerializeField] float tireMass = 20f;

        [Header("Ray")]
        [SerializeField] float rayLength = 1.0f;
        [SerializeField] LayerMask groundMask = ~0;

        private void Awake()
        {
            tire = gameObject.transform;
        }
    
        void FixedUpdate()
        {
            if (!carRb || !tire) return;

            var ray = new Ray(tire.position, -tire.up);
            if (!Physics.Raycast(ray, out _, rayLength, groundMask)) return;

            Vector3 v = carRb.GetPointVelocity(tire.position);
            float lateralVel = Vector3.Dot(tire.right, v);                 // поперечная скорость
            float dv = -lateralVel * grip;                                  // сколько погасить
            float accel = dv / Time.fixedDeltaTime;                         // a = dv/dt
            Vector3 force = tire.right * (tireMass * accel);

            carRb.AddForceAtPosition(force, tire.position);
        }
    }
}
