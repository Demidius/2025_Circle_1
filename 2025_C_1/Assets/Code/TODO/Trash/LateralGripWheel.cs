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

        // для отладки
        private Vector3 _lastForce;
        private Vector3 _lastHitPoint;
        private bool _grounded;

        private void Awake()
        {
            tire = transform;
        }
    
        void FixedUpdate()
        {
            _grounded = false;
            _lastForce = Vector3.zero;
            _lastHitPoint = tire.position;

            if (!carRb || !tire) return;

            var ray = new Ray(tire.position, -tire.up);
            if (!Physics.Raycast(ray, out RaycastHit hit, rayLength, groundMask)) return;

            _grounded = true;
            _lastHitPoint = hit.point;

            Vector3 v = carRb.GetPointVelocity(tire.position);
            float lateralVel = Vector3.Dot(tire.right, v);                 // поперечная скорость
            float dv = -lateralVel * grip;                                 // сколько погасить
            float accel = dv / Time.fixedDeltaTime;                        // a = dv/dt
            _lastForce = tire.right * (tireMass * accel);

            carRb.AddForceAtPosition(_lastForce, tire.position);
        }

        private void OnDrawGizmos()
        {
            if (!tire) tire = transform;

            // рисуем рейкаст вниз
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(tire.position, tire.position - tire.up * rayLength);

            if (_grounded)
            {
                // точка касания с землёй
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_lastHitPoint, 0.05f);

                // вектор силы сцепления
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(tire.position, tire.position + _lastForce / tireMass * 0.1f);

                // вектор поперечной скорости
                if (carRb)
                {
                    Vector3 v = carRb.GetPointVelocity(tire.position);
                    float lateralVel = Vector3.Dot(tire.right, v);
                    Vector3 lateralVec = tire.right * lateralVel;
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(tire.position, tire.position + lateralVec * 0.1f);
                }
            }
        }
    }
}
