using UnityEngine;

namespace Code.GameEntyties.Player.Wheels
{
    public class WheelPosHandler2 : MonoBehaviour
    {
        [Header("Рейкаст")]
        [SerializeField] private float _targetDist = 0.5f;     // желаемая дистанция от колеса до земли
        [SerializeField] private float _rayDistance = 2f;
        [SerializeField] private LayerMask _groundMask = ~0;

        [Header("Ход подвески (от базы)")]
        [SerializeField] private float _maxUp = 0.3f;          // вверх (колесо ближе к корпусу)
        [SerializeField] private float _maxDown = 0.5f;        // вниз (колесо дальше от корпуса)

        [Header("Сглаживание")]
        [SerializeField] private float _smoothTime = 0.08f;
        [SerializeField] private float _maxSpeed = 10f;

        private float _baseLocalY;
        private float _vel; // для SmoothDamp

        private void Start()
        {
            _baseLocalY = transform.localPosition.y;
        }

        private void Update()
        {
            var ray = new Ray(transform.position, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _groundMask, QueryTriggerInteraction.Ignore))
            {
                // положительная ошибка => надо поднять колесо (увеличить Y)
                float error = _targetDist - hit.distance;

                // целевой оффсет относительно базы
                float targetOffset = Mathf.Clamp(error, -_maxUp, _maxDown);

                float currentOffset = transform.localPosition.y - _baseLocalY;
                float newOffset = Mathf.SmoothDamp(currentOffset, targetOffset, ref _vel, _smoothTime, _maxSpeed, Time.deltaTime);

                var local = transform.localPosition;
                local.y = _baseLocalY + newOffset;
                transform.localPosition = local;
            }
            else
            {
                // нет земли под колесом — мягко возвращаемся к базе
                float currentOffset = transform.localPosition.y - _baseLocalY;
                float newOffset = Mathf.SmoothDamp(currentOffset, 0f, ref _vel, _smoothTime, _maxSpeed, Time.deltaTime);

                var local = transform.localPosition;
                local.y = _baseLocalY + newOffset;
                transform.localPosition = local;
            }
        }
    }
}
