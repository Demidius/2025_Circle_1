using System;
using Code.GameEntyties.Player;
using UnityEngine;
using Zenject;

namespace Code.TODO
{
    public class MoverTank : MonoBehaviour
    {
        [Inject] private TanksEngine _tanksEngine;

        [Header("Refs")]
        [SerializeField] private Rigidbody carRb;

        [Header("Ray")]
        [SerializeField] private float rayLength = 1.0f;
        [SerializeField] private LayerMask groundMask = ~0;
        [SerializeField] private Transform _leftSideTransform;
        [SerializeField] private Transform _rightSideTransform;

        [Header("Тяга")]
        [SerializeField] private float _powerMod = 1500f;
        [SerializeField] private float _maxTrackSpeed = 5f;

        [Header("Сопротивления (продольные)")]
        [SerializeField] private float _longitudinalFriction = 0f;
        [SerializeField] private float _coastFriction = 1200f;
        [SerializeField] private float _coastDeadzone = 0.05f;

        [Tooltip("Скорость изменения тяги на гусенице (ед/с)")]
        [SerializeField] private float changeSpeed = 1.5f;

        [Header("Speed / Torque")]
        [SerializeField] private float _topSpeed = 5f;
        [SerializeField] private AnimationCurve availableTorque = new AnimationCurve(
            new Keyframe(0.00f, 0.50f),
            new Keyframe(0.25f, 1.00f),
            new Keyframe(0.75f, 1.00f),
            new Keyframe(1.00f, 0.50f)
        );
        [SerializeField] private float _stopPushEpsilon = 0.2f;

        // params
        private float _leftTrackVelocity;
        private float _rightTrackVelocity;
        private float _leftVelTarget;
        private float _rightVelTarget;

        private bool _engineOn = false;

        // debug
        private Vector3 _lastForce;
        private Vector3 _lastHitPoint;

        private void Start()
        {
            _tanksEngine.ChangeEngineState += SetEngineState;
        }

        private void OnDisable()
        {
            _tanksEngine.ChangeEngineState -= SetEngineState;
        }

        private void Update()
        {
            if (_engineOn)
            {
                LeftTrack();
                RightTrack();
            }
        }

        private void SetEngineState(bool state)
        {
            _engineOn = state;
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
            TrackLogic(_leftSideTransform, _rightTrackVelocity);
            TrackLogic(_rightSideTransform, _leftTrackVelocity);
        }

        private void TrackLogic(Transform t, float velocity01)
        {
            _lastForce = Vector3.zero;
            _lastHitPoint = t ? t.position : transform.position;
            if (!carRb || !t) return;

            var ray = new Ray(t.position, -t.up);
            if (!Physics.Raycast(ray, out RaycastHit hit, rayLength, groundMask, QueryTriggerInteraction.Ignore))
                return;

            _lastHitPoint = hit.point;

            // скорость точки опоры
            Vector3 pointVel = carRb.GetPointVelocity(hit.point);
            float forwardSpeed = Vector3.Dot(pointVel, t.forward); // вдоль ленты

            // целевая скорость ленты
            float targetSpeed = velocity01 * _maxTrackSpeed;
            float speedError = targetSpeed - forwardSpeed;

            // нормализованная скорость машины
            float carForwardSpeed = Vector3.Dot(carRb.linearVelocity, transform.forward);
            float speedNorm = Mathf.Clamp01(Mathf.Abs(carForwardSpeed) / _topSpeed);

            float kTorque = availableTorque.Evaluate(speedNorm);

            bool atTopAndAccelerating =
                (Mathf.Abs(carForwardSpeed) >= (_topSpeed - _stopPushEpsilon)) && (speedError > 0f);
            if (atTopAndAccelerating) kTorque = 0f;

            // базовая тяга по ошибке
            Vector3 driveForce = t.forward * (speedError * _powerMod * kTorque);

            // продольное демпфирование:
            // 1) когда есть инпут — слабое
            if (_longitudinalFriction > 0f && Mathf.Abs(velocity01) > _coastDeadzone)
                driveForce += -t.forward * (forwardSpeed * _longitudinalFriction * kTorque);

            // 2) когда инпут ≈ 0 — сильнее тянем к нулю (накат гасим)
            if (Mathf.Abs(velocity01) <= _coastDeadzone && _coastFriction > 0f)
                driveForce += -t.forward * (forwardSpeed * _coastFriction);

            _lastForce = driveForce;
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
            Gizmos.DrawRay(_lastHitPoint, _lastForce * 0.001f);
        }
#endif
    }
}
