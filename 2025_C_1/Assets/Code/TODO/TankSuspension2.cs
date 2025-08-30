using UnityEngine;

namespace Code.TODO
{
    public class TankSuspension2 : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Rigidbody _tankRigidBody;
        [SerializeField] private Transform[] _tireTransform;
        [SerializeField] private GameObject[] _wheels;

        [Header("Suspension Settings")]
        [SerializeField] private float suspensionRestDist = 0.5f;
        [SerializeField] private float springStrength = 20000f; // жёсткость пружины
        [SerializeField] private float springDamper = 1500f; // демпфер
        [SerializeField] private float rayLength = 1.0f; // длина рейкаста
        [SerializeField] LayerMask _layerMask;

        [SerializeField]
        private float offset = 1f;

        private void FixedUpdate()
        {
            for (int i = 0; i < _tireTransform.Length; i++)
            {

                Vector3 springDir = _tireTransform[i].up;

                Vector3 down = -springDir;

                Ray tireRay = new Ray(_tireTransform[i].position, down);

                if (Physics.Raycast(tireRay, out RaycastHit hit, rayLength, _layerMask))
                {

                    Vector3 tireWorldVel = _tankRigidBody.GetPointVelocity(_tireTransform[i].position);

                    float offset = suspensionRestDist - hit.distance;

                    float vel = Vector3.Dot(springDir, tireWorldVel);

                    float force = (offset * springStrength) - (vel * springDamper);

                    _tankRigidBody.AddForceAtPosition(springDir * force, _tireTransform[i].position, ForceMode.Impulse);

                    if (i == 0 || i == 8)
                    {

                    }
                    else
                    {
                        _wheels[i].transform.position = hit.point + new Vector3(0, 0.19f, 0);
                    }
                }
            }
        }

        [Header("Gizmos")]
        [SerializeField] private bool _drawGizmos = true;
        [SerializeField, Range(0.00001f, 0.01f)]
        private float _forceGizmoScale = 0.0001f; // масштаб стрелки силы
        [SerializeField] private Color _rayColor = new Color(0.1f, 0.6f, 1f, 0.8f);
        [SerializeField] private Color _restPointColor = new Color(1f, 0.9f, 0.1f, 0.9f);
        [SerializeField] private Color _hitColor = new Color(0.2f, 1f, 0.2f, 0.9f);
        [SerializeField] private Color _forceColor = new Color(1f, 0.3f, 0.2f, 0.9f);

        private void OnDrawGizmos()
        {
            if (!_drawGizmos || _tireTransform == null) return;

            for (int i = 0; i < _tireTransform.Length; i++)
            {
                var t = _tireTransform[i];
                if (t == null) continue;

                Vector3 springDir = t.up;
                Vector3 down = -springDir;
                Vector3 origin = t.position;

                // РЕЙКАСТ (луч подвески)
                Gizmos.color = _rayColor;
                Gizmos.DrawLine(origin, origin + down * rayLength);
                Gizmos.DrawWireSphere(origin, 0.03f);

                // Точка покоя (rest length)
                Vector3 restPoint = origin + down * suspensionRestDist;
                Gizmos.color = _restPointColor;
                Gizmos.DrawWireSphere(restPoint, 0.04f);

                // Попробуем сделать реальный рейкаст, чтобы показать хит и силу
                if (Physics.Raycast(origin, down, out RaycastHit hit, rayLength, _layerMask, QueryTriggerInteraction.Ignore))
                {
                    // Контакт с землёй
                    Gizmos.color = _hitColor;
                    Gizmos.DrawSphere(hit.point, 0.05f);
                    Gizmos.DrawLine(origin, hit.point);

                    // Оценка силы (та же формула, что в FixedUpdate)
                    float offsetNow = suspensionRestDist - hit.distance;

                    float vel = 0f;
                    if (_tankRigidBody != null)
                    {
                        // В редакторе вне Play скорость будет 0 — это ок
                        Vector3 tireWorldVel = _tankRigidBody.GetPointVelocity(origin);
                        vel = Vector3.Dot(springDir, tireWorldVel);
                    }

                    float force = (offsetNow * springStrength) - (vel * springDamper);

                    // Стрелка силы пружины (вверх по springDir; размер масштабируем)
                    Vector3 forceVec = springDir * force * _forceGizmoScale;
                    DrawArrow(origin, forceVec, _forceColor, 0.15f, 18f);
                }
            }
        }

        // Вспомогательная отрисовка стрелки
        private void DrawArrow(Vector3 start, Vector3 vec, Color col, float headSize = 0.15f, float headAngle = 20f)
        {
            Gizmos.color = col;
            Vector3 end = start + vec;
            Gizmos.DrawLine(start, end);

            if (vec.sqrMagnitude < 1e-6f) return;

            // головка стрелки
            Vector3 dir = vec.normalized;
            Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180 + headAngle, 0) * Vector3.forward;
            Vector3 left = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180 - headAngle, 0) * Vector3.forward;

            Gizmos.DrawLine(end, end + right * headSize);
            Gizmos.DrawLine(end, end + left * headSize);
        }
    }

}

