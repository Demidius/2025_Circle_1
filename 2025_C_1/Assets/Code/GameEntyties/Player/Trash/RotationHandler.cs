using System;
using UnityEngine;
using Zenject;

namespace Code.GameEntyties.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class RotationHandler : MonoBehaviour
    {
        [Inject] private ITanksEngine _tanksEngine;
        [Inject] private ITrackVelocity _trackVelocity; 

        private Rigidbody _rb;

        [Header("Скорость поворота, град/с при (Right-Left)=1")]
        [SerializeField] private float _speedOfRotation = 30f;

        private float _engineRpm; 

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _tanksEngine.ChangeRpm += UpdateRpm;
        }

        private void OnDisable()
        {
            _tanksEngine.ChangeRpm -= UpdateRpm;
        }

        private void FixedUpdate()
        {
            // Debug.Log(_trackVelocity.LeftTrackVelocity+ "  " + _trackVelocity.RightTrackVelocity);
            
            // Разница скоростей гусениц: >0 — поворот вправо, <0 — влево
            float turnInput = _trackVelocity.LeftTrackVelocity - _trackVelocity.RightTrackVelocity;

            // Угловая скорость в град/с
            float yawSpeedDegPerSec = turnInput * _speedOfRotation * _engineRpm / 1000;

            // Приращение угла за кадр физики
            float deltaYawDeg = yawSpeedDegPerSec * Time.fixedDeltaTime;

            // Поворот вокруг оси Y
            Quaternion deltaRot = Quaternion.AngleAxis(deltaYawDeg, Vector3.up);
            _rb.MoveRotation(deltaRot * _rb.rotation);
        }

        private void UpdateRpm(float engineRpm)
        {
            _engineRpm = engineRpm;
            // при желании можно масштабировать поворот мощностью двигателя:

        }
    }
}
