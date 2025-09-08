using UnityEngine;

namespace Code.GameEntyties.Player.Trash
{
    public class SideCorect : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Rigidbody _rbTank;
        [SerializeField] private Transform[] _points;

        [Header("Ground")]
        [SerializeField] private float _rayLength = 1f;
        [SerializeField] private LayerMask _groundMask = ~0;

        [Header("Friction")]
        [SerializeField] private float _lateralFriction = 1f;
        const float stickVel = 1f;         // м/с — порог «стоя на месте»
        const float stickTime = 0.1f;      // с — за сколько гасим v

        [Header("Gizmos")]
        [SerializeField] private bool _drawGizmos = true;          // вкл/выкл гизмо
        [SerializeField] private bool _gizmosOnlyWhenSelected = true; // рисовать только при выделении
        [SerializeField] private float _vecScale = 0.5f;           // масштаб стрелок

        private void FixedUpdate()
        {
            for (int i = 0; i < _points.Length; i++)
            {
                var p = _points[i];
                var ray = new Ray(p.position, -p.up);

                if (!Physics.Raycast(ray, out RaycastHit hit, _rayLength, _groundMask, QueryTriggerInteraction.Ignore))
                    continue;

                Vector3 lateralDir = Vector3.ProjectOnPlane(p.right, hit.normal).normalized;

                Vector3 pointVel = Vector3.zero;
                if (_rbTank && Application.isPlaying)
                    pointVel = _rbTank.GetPointVelocity(hit.point);

                float lateralSpeed = Vector3.Dot(pointVel, lateralDir);

                if (Mathf.Abs(lateralSpeed) < stickVel)
                {
                    float a = -lateralSpeed / stickTime; // a = -v/T
                    _rbTank.AddForceAtPosition(lateralDir * (_rbTank.mass * a), hit.point, ForceMode.Force);
                }
                else
                {
                    float forceMag = Mathf.Clamp(-lateralSpeed * _lateralFriction, -5000f, 5000f);
                    _rbTank.AddForceAtPosition(lateralDir * forceMag, hit.point, ForceMode.Force);
                }
            }
        }

        #region Gizmos
        private void OnDrawGizmos()
        {
            if (_drawGizmos && !_gizmosOnlyWhenSelected) DrawGizmosInternal();
        }

        private void OnDrawGizmosSelected()
        {
            if (_drawGizmos && _gizmosOnlyWhenSelected) DrawGizmosInternal();
        }

        private void DrawGizmosInternal()
        {
            if (_points == null) return;

            Gizmos.matrix = Matrix4x4.identity;

            foreach (var p in _points)
            {
                if (!p) continue;

                // Луч вниз
                Vector3 rayDir = -p.up;
                Gizmos.color = new Color(0.2f, 0.6f, 1f); // голубой — луч
                Gizmos.DrawLine(p.position, p.position + rayDir * _rayLength);

                // Хит
                if (Physics.Raycast(p.position, rayDir, out RaycastHit hit, _rayLength, _groundMask, QueryTriggerInteraction.Ignore))
                {
                    // Точка касания
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(hit.point, 0.03f);

                    // Нормаль
                    Gizmos.color = Color.green;
                    DrawArrow(hit.point, hit.normal, 0.25f);

                    // Боковое направление (на поверхности)
                    Vector3 lateralDir = Vector3.ProjectOnPlane(p.right, hit.normal).normalized;
                    Gizmos.color = Color.magenta;
                    DrawArrow(hit.point, lateralDir, 0.35f);

                    // Скорость точки (только в Play)
                    if (_rbTank && Application.isPlaying)
                    {
                        Vector3 v = _rbTank.GetPointVelocity(hit.point);
                        Gizmos.color = new Color(1f, 0.5f, 0f); // оранжевый — скорость точки
                        DrawArrow(hit.point, v, _vecScale);
                    }
                }
            }
        }

        private void DrawArrow(Vector3 from, Vector3 dir, float length)
        {
            if (dir.sqrMagnitude < 1e-6f) return;
            Vector3 to = from + dir.normalized * length;
            Gizmos.DrawLine(from, to);

            // наконечник
            Vector3 back = (from + to) * 0.5f;
            Vector3 side = Vector3.Cross(dir.normalized, Vector3.up);
            if (side.sqrMagnitude < 1e-6f) side = Vector3.right; // на случай вертикали
            side.Normalize();
            float head = length * 0.2f;
            Gizmos.DrawLine(to, to - dir.normalized * head + side * head * 0.5f);
            Gizmos.DrawLine(to, to - dir.normalized * head - side * head * 0.5f);
        }
        #endregion
    }
}
