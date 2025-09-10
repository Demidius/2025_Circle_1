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
        [SerializeField] float _longitudinalFriction = 0f;
        [SerializeField] float _coastFriction = 1200f;
        [SerializeField] float _coastDeadzone = 0.05f;

        [SerializeField] float changeSpeed = 1.5f;

        [Header("Speed / Torque")]
        [SerializeField] float _topSpeed = 5f;
        [SerializeField] AnimationCurve availableTorque = new AnimationCurve(
            new Keyframe(0.00f, 0.50f),
            new Keyframe(0.25f, 1.00f),
            new Keyframe(0.75f, 1.00f),
            new Keyframe(1.00f, 0.50f)
        );
        [SerializeField] float _stopPushEpsilon = 0.2f;

        float _leftTrackVelocity, _rightTrackVelocity;
        float _leftVelTarget, _rightVelTarget;

        Vector3 _lastForce;
        Vector3 _lastHitPoint;

        void Update()
        {
            if (!_telemetry.EngineOn) return;

            float t = _telemetry.Throttle01;
            float r = _telemetry.Turn01;

            _leftVelTarget  = Mathf.Clamp(t - r, -1f, 1f);
            _rightVelTarget = Mathf.Clamp(t + r, -1f, 1f);

            _leftTrackVelocity  = Mathf.MoveTowards(_leftTrackVelocity,  _leftVelTarget,  changeSpeed * Time.deltaTime);
            _rightTrackVelocity = Mathf.MoveTowards(_rightTrackVelocity, _rightVelTarget, changeSpeed * Time.deltaTime);
        }

        void FixedUpdate()
        {
            TrackLogic(_leftSideTransform,  _leftTrackVelocity);
            TrackLogic(_rightSideTransform, _rightTrackVelocity);
        }

        void TrackLogic(Transform t, float velocity01)
        {
            _lastForce = Vector3.zero;
            _lastHitPoint = t ? t.position : transform.position;
            if (!carRb || !t) return;

            var ray = new Ray(t.position, -t.up);
            if (!Physics.Raycast(ray, out RaycastHit hit, rayLength, groundMask, QueryTriggerInteraction.Ignore))
                return;

            _lastHitPoint = hit.point;

            Vector3 pointVel = carRb.GetPointVelocity(hit.point);
            float forwardSpeed = Vector3.Dot(pointVel, t.forward);

            float targetSpeed = velocity01 * _maxTrackSpeed;
            float speedError = targetSpeed - forwardSpeed;

            float carForwardSpeed = Vector3.Dot(carRb.linearVelocity, transform.forward);
            float speedNorm = Mathf.Clamp01(Mathf.Abs(carForwardSpeed) / _topSpeed);
            float kTorque = availableTorque.Evaluate(speedNorm);

            bool atTopAndAccelerating =
                (Mathf.Abs(carForwardSpeed) >= (_topSpeed - _stopPushEpsilon)) && (speedError > 0f);
            if (atTopAndAccelerating) kTorque = 0f;

            Vector3 driveForce = t.forward * (speedError * _powerMod * kTorque);

            if (_longitudinalFriction > 0f && Mathf.Abs(velocity01) > _coastDeadzone)
                driveForce += -t.forward * (forwardSpeed * _longitudinalFriction * kTorque);

            if (Mathf.Abs(velocity01) <= _coastDeadzone && _coastFriction > 0f)
                driveForce += -t.forward * (forwardSpeed * _coastFriction);

            _lastForce = driveForce;
            carRb.AddForceAtPosition(_lastForce, hit.point, ForceMode.Force);
        }
    }
}
