using UnityEngine;

namespace Code.TODO
{
    public class Mover2 : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Rigidbody carRb;

        [Header("Ray")]
        [SerializeField] private float rayLength = 1.0f;
        [SerializeField] private LayerMask groundMask = ~0;

        [SerializeField] private Transform _leftSideTransform;
        [SerializeField] private Transform _rightSideTransform;

        [Header("Тяга")]
        [Tooltip("Ньютонов на (м/с) ошибки скорости")]
        [SerializeField] private float _powerMod = 1500f;
        [Tooltip("Макс целевая скорость ленты гусеницы (м/с) для vel=±1")]
        [SerializeField] private float _maxTrackSpeed = 5f;

        [Header("Сопротивления")]
        [Tooltip("Демпфирование продольной ошибки (усиление уже учтено в _powerMod)")]
        [SerializeField] private float _longitudinalFriction = 0f; // можно оставить 0
        [Tooltip("Демпфирование бокового сноса точки опоры")]
        [SerializeField] private float _lateralFriction = 800f;

        [Tooltip("Скорость изменения тяги на гусенице (ед/с)")]
        [SerializeField] private float changeSpeed = 1.5f;

        // params
        private float _leftTrackVelocity;   // [-1..1]
        private float _rightTrackVelocity;  // [-1..1]
        private float _leftVelTarget;
        private float _rightVelTarget;

        // debug
        private Vector3 _lastForce;
        private Vector3 _lastHitPoint;

        private void Update()
        {
            LeftTrack();
            RightTrack();
        }

        private void LeftTrack()
        {
            _leftVelTarget =
                (Input.GetKey(KeyCode.Q) ? 1f : 0f) +
                (Input.GetKey(KeyCode.A) ? -1f : 0f);

            _leftVelTarget = Mathf.Clamp(_leftVelTarget, -1f, 1f);
            _leftTrackVelocity = Mathf.MoveTowards(_leftTrackVelocity, _leftVelTarget, changeSpeed * Time.deltaTime);
        }

        private void RightTrack()
        {
            _rightVelTarget =
                (Input.GetKey(KeyCode.E) ? 1f : 0f) +
                (Input.GetKey(KeyCode.D) ? -1f : 0f);

            _rightVelTarget = Mathf.Clamp(_rightVelTarget, -1f, 1f);
            _rightTrackVelocity = Mathf.MoveTowards(_rightTrackVelocity, _rightVelTarget, changeSpeed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            // Компенсировать «естественную» скорость: считаем скорость точки контакта на земле
            // и тянем её к целевой скорости ленты для каждой гусеницы.
            TrackLogic(_leftSideTransform, _rightTrackVelocity );
            TrackLogic(_rightSideTransform, _leftTrackVelocity);
        }

        private void TrackLogic(Transform t, float velocity01)
        {
            _lastForce = Vector3.zero;
            _lastHitPoint = t ? t.position : transform.position;

            if (!carRb || !t || !_rightSideTransform) return;

            var ray = new Ray(t.position, -t.up);

            if (!Physics.Raycast(ray, out RaycastHit hit, rayLength, groundMask, QueryTriggerInteraction.Ignore))
                return;

            _lastHitPoint = hit.point;

            // 1) Текущая скорость точки опоры в мировых координатах
            Vector3 pointVel = carRb.GetPointVelocity(hit.point);

            // Разложим на продольную и поперечную составляющие относительно гусеницы
            float forwardSpeed = Vector3.Dot(pointVel, t.forward); // м/с вдоль ленты
            float lateralSpeed = Vector3.Dot(pointVel, t.right);   // м/с поперёк ленты

            // 2) Целевая продольная скорость ленты от инпута
            float targetSpeed = velocity01 * _maxTrackSpeed; // м/с

            // 3) Ошибка по продольной скорости: хотим forwardSpeed -> targetSpeed
            float speedError = targetSpeed - forwardSpeed;

            // 4) Силы:
            // Тяга вдоль ленты, пропорциональна ошибке скорости (компенсирует инерцию/естественный накат)
            Vector3 driveForce = t.forward * (speedError * _powerMod);

            // Небольшое продольное демпфирование (опционально)
            if (_longitudinalFriction > 0f)
                driveForce += -t.forward * (forwardSpeed * _longitudinalFriction);

            // Боковое демпфирование (срезаем снос)
            Vector3 lateralForce = -t.right * (lateralSpeed * _lateralFriction);

            _lastForce = driveForce + lateralForce;

            carRb.AddForceAtPosition(_lastForce, hit.point, ForceMode.Force);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            if (_leftSideTransform)
                Gizmos.DrawLine(_leftSideTransform.position, _leftSideTransform.position - _leftSideTransform.up * rayLength);
            if (_rightSideTransform)
                Gizmos.DrawLine(_rightSideTransform.position, _rightSideTransform.position - _rightSideTransform.up * rayLength);

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(_lastHitPoint, 0.05f);
            Gizmos.DrawRay(_lastHitPoint, _lastForce * 0.001f); // масштаб для наглядности
        }
#endif
    }
}
