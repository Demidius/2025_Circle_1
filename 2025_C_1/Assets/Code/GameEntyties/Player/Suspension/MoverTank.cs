using Code.GameEntities.Vehicle;
using Code.TODO;
using UnityEngine;
using Zenject;

namespace Code.GameEntities.Player.Suspension
{
    public sealed class MoverTank : MonoBehaviour
    {
        [Inject] ITankTelemetryReadOnly _telemetry;

        [SerializeField] Rigidbody carRb;
        [SerializeField] float rayLength = 1.0f;
        [SerializeField] LayerMask groundMask = ~0;
        [SerializeField] Transform _leftSideTransform;
        [SerializeField] Transform _rightSideTransform;

        [Header("Тяга")]
        [SerializeField] float _powerMod = 1500f;
        [SerializeField] float _maxTrackSpeed = 5f;

        [Header("Сопрот. продольные")]
        [SerializeField] float _longitudinalFriction = 0f;   // активное торможение при газе
        [SerializeField] float _coastFriction = 1200f;       // накат (когда газ в мёртвой зоне)
        [SerializeField] float _coastDeadzone = 0.05f;

        [SerializeField] float changeSpeed = 1.5f;           // скорость изменения команды на гусеницу (в ед/с)

        [Header("Speed / Torque")]
        [SerializeField] float _topSpeed = 5f;
        [SerializeField] AnimationCurve availableTorque = new AnimationCurve(
            new Keyframe(0.00f, 0.50f),
            new Keyframe(0.25f, 1.00f),
            new Keyframe(0.75f, 1.00f),
            new Keyframe(1.00f, 0.50f)
        );
        [SerializeField] float _stopPushEpsilon = 0.2f;

        float _leftTrackVelocity, _rightTrackVelocity;   // сглаженные команды [-1..1]
        float _leftVelTarget, _rightVelTarget;           // целевые команды [-1..1]

        Vector3 _lastForceL, _lastForceR;

        void Update()
        {
            if (_telemetry.EngineOn)
            {
                _leftVelTarget  = Mathf.Clamp(_telemetry.LeftTrack01,  -1f, 1f);
                _rightVelTarget = Mathf.Clamp(_telemetry.RightTrack01, -1f, 1f);
            }
            else
            {
                
                _leftVelTarget  = Mathf.MoveTowards(_leftVelTarget,  0f, changeSpeed * Time.deltaTime);
                _rightVelTarget = Mathf.MoveTowards(_rightVelTarget, 0f, changeSpeed * Time.deltaTime);
            }
        }

        void FixedUpdate()
        {
            // сглаживание команд в физ. тике — без рассинхрона
            float dt = Time.fixedDeltaTime;
            _leftTrackVelocity  = Mathf.MoveTowards(_leftTrackVelocity,  _leftVelTarget,  changeSpeed * dt);
            _rightTrackVelocity = Mathf.MoveTowards(_rightTrackVelocity, _rightVelTarget, changeSpeed * dt);

            // применение сил
            _lastForceL = TrackLogic(_leftSideTransform,  _leftTrackVelocity);
            _lastForceR = TrackLogic(_rightSideTransform, _rightTrackVelocity);
        }

        // Возвращаем силу для дебага
        Vector3 TrackLogic(Transform t, float velocity01)
        {
            if (!carRb || !t) return Vector3.zero;

            var ray = new Ray(t.position, -t.up);
            if (!Physics.Raycast(ray, out RaycastHit hit, rayLength, groundMask, QueryTriggerInteraction.Ignore))
                return Vector3.zero;

            // скорость точки контакта вдоль направления гусеницы
            Vector3 pointVel = carRb.GetPointVelocity(hit.point);
            float forwardSpeed = Vector3.Dot(pointVel, t.forward);

            // целевая линейная скорость дорожки
            float targetSpeed = Mathf.Clamp(velocity01, -1f, 1f) * _maxTrackSpeed;
            float speedError = targetSpeed - forwardSpeed;

            // нормированная скорость шасси (для кривой момента)
            float carForwardSpeed = Vector3.Dot(carRb.linearVelocity, transform.forward);
            float speedNorm = Mathf.Clamp01(Mathf.Abs(carForwardSpeed) / _topSpeed);
            float kTorque = availableTorque.Evaluate(speedNorm);

            // не толкаем дальше топ-спида, но сохраним трения
            bool atTopAndAccelerating =
                (Mathf.Abs(carForwardSpeed) >= (_topSpeed - _stopPushEpsilon)) && (speedError > 0f);
            float driveTorque = atTopAndAccelerating ? 0f : (speedError * _powerMod * kTorque);

            Vector3 force = t.forward * driveTorque;

            // продольное трение при нажатом газе — НЕ завязываем на kTorque (чтобы не пропадало на Vmax)
            if (_longitudinalFriction > 0f && Mathf.Abs(velocity01) > _coastDeadzone)
                force += -t.forward * (forwardSpeed * _longitudinalFriction);

            // трение накатом — всегда
            if (Mathf.Abs(velocity01) <= _coastDeadzone && _coastFriction > 0f)
                force += -t.forward * (forwardSpeed * _coastFriction);

            carRb.AddForceAtPosition(force, hit.point, ForceMode.Force);
            return force;
        }

        void OnDrawGizmosSelected()
        {
            // простые гизмо для дебага лучей и последних сил
            Gizmos.color = Color.yellow;
            if (_leftSideTransform)  Gizmos.DrawLine(_leftSideTransform.position,  _leftSideTransform.position  - _leftSideTransform.up * rayLength);
            if (_rightSideTransform) Gizmos.DrawLine(_rightSideTransform.position, _rightSideTransform.position - _rightSideTransform.up * rayLength);

            Gizmos.color = Color.cyan;
            if (_leftSideTransform)  Gizmos.DrawRay(_leftSideTransform.position,  _lastForceL * 0.001f);
            if (_rightSideTransform) Gizmos.DrawRay(_rightSideTransform.position, _lastForceR * 0.001f);
        }
    }
}
