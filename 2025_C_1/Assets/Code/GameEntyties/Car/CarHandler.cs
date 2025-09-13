using System;
using UnityEngine;

namespace Code.GameEntyties.Car
{
    public class CarHandler : MonoBehaviour
    {
        public enum DriveType { FWD, RWD, AWD }

        [Header("Привод и физика")]
        [SerializeField] private DriveType _driveType = DriveType.RWD;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Vector3 _centerOfMassOffset;

        [Header("Силы")]
        [SerializeField] private float _motorForce = 1500f;
        [SerializeField] private float _brakeForce = 3000f;
        [SerializeField] private float _maxSteerAngle = 30f;

        [Header("Колёса (коллайдеры)")]
        [SerializeField] private WheelCollider _frontLeftWheel;
        [SerializeField] private WheelCollider _frontRightWheel;
        [SerializeField] private WheelCollider _rearLeftWheel;
        [SerializeField] private WheelCollider _rearRightWheel;

        [Header("Колёса (визуал)")]
        [SerializeField] private Transform _frontLeftWheelTransform;
        [SerializeField] private Transform _frontRightWheelTransform;
        [SerializeField] private Transform _rearLeftWheelTransform;
        [SerializeField] private Transform _rearRightWheelTransform;

        [SerializeField] private Quaternion _wheelRotOffset = Quaternion.identity;

        [Header("Старый инпут")]
        [SerializeField] private string _axisHorizontal = "Horizontal"; // A/D, ←/→
        [SerializeField] private string _axisVertical   = "Vertical";   // W/S, ↑/↓
        [SerializeField] private KeyCode _brakeKey = KeyCode.Space;     // тормоз

        private float _throttle;   // -1..1
        private float _steer;      // -1..1
        private bool _isBraking;

        private void Awake()
        {
            if (_rb == null) _rb = GetComponentInParent<Rigidbody>();
            if (_rb != null && _centerOfMassOffset != Vector3.zero)
                _rb.centerOfMass += _centerOfMassOffset;
        }

        private void Update()
        {
            // ----- Чтение ввода (СТАРЫЙ Input Manager) -----
            _steer = Mathf.Clamp(Input.GetAxis(_axisHorizontal), -1f, 1f);
            _throttle = Mathf.Clamp(Input.GetAxis(_axisVertical), -1f, 1f);
            _isBraking = Input.GetKey(_brakeKey);
        }

        private void FixedUpdate()
        {
            ApplySteer();
            ApplyMotor();
            ApplyBrakes();
            UpdateWheelsVisual();
        }

        // ----- Управление -----

        private void ApplySteer()
        {
            float angle = _steer * _maxSteerAngle;
            _frontLeftWheel.steerAngle = angle;
            _frontRightWheel.steerAngle = angle;
        }

        private void ApplyMotor()
        {
            // Если тормоз зажат — глушим моторный момент
            float torque = _isBraking ? 0f : (_throttle * _motorForce);

            switch (_driveType)
            {
                case DriveType.FWD:
                    _frontLeftWheel.motorTorque  = torque;
                    _frontRightWheel.motorTorque = torque;
                    _rearLeftWheel.motorTorque   = 0f;
                    _rearRightWheel.motorTorque  = 0f;
                    break;

                case DriveType.RWD:
                    _rearLeftWheel.motorTorque   = torque;
                    _rearRightWheel.motorTorque  = torque;
                    _frontLeftWheel.motorTorque  = 0f;
                    _frontRightWheel.motorTorque = 0f;
                    break;

                case DriveType.AWD:
                    float half = torque * 0.5f;
                    _frontLeftWheel.motorTorque  = half;
                    _frontRightWheel.motorTorque = half;
                    _rearLeftWheel.motorTorque   = half;
                    _rearRightWheel.motorTorque  = half;
                    break;
            }
        }

        private void ApplyBrakes()
        {
            float brake = _isBraking ? _brakeForce : 0f;
            _frontLeftWheel.brakeTorque  = brake;
            _frontRightWheel.brakeTorque = brake;
            _rearLeftWheel.brakeTorque   = brake;
            _rearRightWheel.brakeTorque  = brake;
        }

        // ----- Визуал -----

        private void UpdateWheelsVisual()
        {
            UpdateSingleWheel(_frontLeftWheel,  _frontLeftWheelTransform,  _wheelRotOffset);
            UpdateSingleWheel(_frontRightWheel, _frontRightWheelTransform, _wheelRotOffset);
            UpdateSingleWheel(_rearLeftWheel,   _rearLeftWheelTransform,   _wheelRotOffset);
            UpdateSingleWheel(_rearRightWheel,  _rearRightWheelTransform,  _wheelRotOffset);
        }

        private static void UpdateSingleWheel(WheelCollider wc, Transform visual, Quaternion rotOffset)
        {
            if (wc == null || visual == null) return;
            wc.GetWorldPose(out Vector3 pos, out Quaternion rot);
            visual.SetPositionAndRotation(pos, rot * rotOffset);
        }
    }
}
