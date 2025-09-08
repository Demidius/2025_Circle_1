using UnityEngine;

namespace TODO.Trash
{
    public class TireSteeringForceMulti : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Rigidbody carRigidBody;
        [SerializeField] private Transform[] tirePoints; // точки (колёса/сайлентблоки)

        [Header("Params")]
        [SerializeField] private float tireMassPerPoint = 20f;   // «масса» на точку
        [SerializeField] private float tireGripFactor = 1.0f;    // 0..1

        [Header("Raycast")]
        [SerializeField] private float rayLength = 0.5f;
        [SerializeField] private LayerMask groundMask = ~0;

        private void FixedUpdate()
        {
            if (carRigidBody == null || tirePoints == null) return;

            for (int i = 0; i < tirePoints.Length; i++)
            {
                var t = tirePoints[i];
                if (t == null) continue;

                // проверка соприкосновения с землёй
                if (!Physics.Raycast(t.position, -t.up, out _, rayLength, groundMask, QueryTriggerInteraction.Ignore))
                    continue;

                // направление боковой (рулящей) силы в мировых координатах
                Vector3 steeringDir = t.right;

                // скорость точки подвески
                Vector3 tireWorldVel = carRigidBody.GetPointVelocity(t.position);

                // проекция скорости на направление боковой силы
                float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);

                // нужно изменить скорость на величину против хода, с учётом сцепления
                float desiredVelChange = -steeringVel * tireGripFactor;

                // ускорение для изменения скорости за один физ. шаг
                float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

                // F = m * a, применяем в точке
                Vector3 force = steeringDir * tireMassPerPoint * desiredAccel;
                carRigidBody.AddForceAtPosition(force, t.position, ForceMode.Force);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (tirePoints == null) return;
            Gizmos.color = Color.yellow;
            for (int i = 0; i < tirePoints.Length; i++)
            {
                var t = tirePoints[i];
                if (t == null) continue;
                Gizmos.DrawLine(t.position, t.position - t.up * rayLength);
            }
        }
#endif
    }
}
