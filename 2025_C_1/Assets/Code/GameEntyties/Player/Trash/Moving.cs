using UnityEngine;
using Zenject;

namespace Code.GameEntyties.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Moving : MonoBehaviour
    {
        [Inject] private ITanksEngine _tanksEngine;
        [Inject] private ITrackVelocity _trackVelocity;

        private Rigidbody _rb;

        // сглаженное значение целевой «линейной» тяги
        private float _tankSpeed;

        [Header("Параметры")]
        [Tooltip("Скорость сглаживания изменения тяги (ед/с)")]
        [SerializeField] private float accel = 3f;

        [Tooltip("Коэф. перевода RPM в скорость")]
        [SerializeField] private float rpmToSpeed = 0.001f;
    
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {

            float left = _trackVelocity.LeftTrackVelocity;
            float right = _trackVelocity.RightTrackVelocity;

            //   среднее
            float targetSpeed = 0.5f * (left + right);
            // float targetSpeed = Mathf.Abs(left) < Mathf.Abs(right) ? left : right;

            // Сгладить к целевому значению (ед/с * dt)
            _tankSpeed = Mathf.MoveTowards(_tankSpeed, targetSpeed, accel * _rb.mass / 1000 * Time.fixedDeltaTime);

            // Переводим RPM двигателя в масштаб скорости
            float engineFactor = _tanksEngine.EngineRpm * rpmToSpeed;

            // Линейное смещение вперёд
            Vector3 move = transform.forward * _tankSpeed * engineFactor * Time.fixedDeltaTime;
            _rb.MovePosition(_rb.position + move);
        }
    }
}
