using Code.TODO;
using UnityEngine;
using Zenject;

namespace Code.GameEntities.Vehicle
{
    public sealed class TankKinematics : MonoBehaviour
    {
        [SerializeField] Rigidbody _rb;
        [SerializeField] Transform _forwardRef;
        [SerializeField] float _topSpeed = 5f;
        [SerializeField] float _accelSmooth = 12f;

        ITankTelemetry _telemetry;
        float _prevForwardSpeed;
        float _smoothedAccel;

        [Inject] void Construct(ITankTelemetry telemetry) => _telemetry = telemetry;

        void Reset()
        {
            if (!_rb) _rb = GetComponent<Rigidbody>();
            if (!_forwardRef) _forwardRef = transform;
        }

        void FixedUpdate()
        {
            if (!_rb || !_forwardRef) return;

            float forwardSpeed = Vector3.Dot(_rb.linearVelocity, _forwardRef.forward);
            float speed01 = Mathf.Clamp01(Mathf.Abs(forwardSpeed) / Mathf.Max(_topSpeed, 0.01f));

            float accel = (forwardSpeed - _prevForwardSpeed) / Mathf.Max(Time.fixedDeltaTime, 0.0001f);
            _prevForwardSpeed = forwardSpeed;
            _smoothedAccel = Mathf.Lerp(_smoothedAccel, accel, 1f - Mathf.Exp(-_accelSmooth * Time.fixedDeltaTime));

            float lateral = Vector3.Dot(_rb.linearVelocity, _forwardRef.right);
            float slip01 = Mathf.Clamp01(Mathf.Abs(lateral) / (Mathf.Abs(forwardSpeed) + 0.1f));

            _telemetry.SetKinematics(forwardSpeed, speed01, _smoothedAccel, slip01);
        }
    }
}
