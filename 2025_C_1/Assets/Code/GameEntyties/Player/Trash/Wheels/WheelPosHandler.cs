using UnityEngine;

namespace Code.GameEntyties.Player.Wheels
{
    public class WheelPosHandler : MonoBehaviour
    {
        [Header("Рейкаст")]
        [SerializeField] private float _rayDistance = 5f;
        [SerializeField] private LayerMask _groundMask = ~0;
        [Tooltip("Локальное направление луча (например, -Z)")]
        [SerializeField] private Vector3 _rayLocalDir = new Vector3(0, 0, -1); // по Z

        [Header("Подвеска по Z")]
        [SerializeField] private float _baseLocalZ = 0.9f;   // базовая локальная позиция по Z
        [SerializeField] private float _smoothTime = 0.08f;  // время сглаживания
        [SerializeField] private float _minZ = 0.0f;         // ограничители хода
        [SerializeField] private float _maxZ = 1.5f;

        private float _targetLocalZ;
        private float _velZ; // для SmoothDamp

        private void Awake()
        {
            _targetLocalZ = _baseLocalZ; // стартовая цель
        }

        private void Update()
        {
            // Луч в мировых координатах по локальному направлению
            Vector3 dirWS = transform.TransformDirection(_rayLocalDir.normalized);

            if (Physics.Raycast(transform.position, dirWS, out RaycastHit hit, _rayDistance, _groundMask, QueryTriggerInteraction.Ignore))
            {
                // Хотим держать колесо на расстоянии от поверхности вдоль выбранной оси.
                // Простейшая логика: чем ближе пол, тем "выше" уводим колесо по Z (или наоборот — зависит от ориентации модели).
                // Ниже вариант: целевой Z равен расстоянию до пола, прижатый к лимитам и с базовым сдвигом.
                float desired = _baseLocalZ - (hit.distance - _baseLocalZ);
                _targetLocalZ = Mathf.Clamp(desired, _minZ, _maxZ);
            }
            else
            {
                // Пол не нашли — мягко возвращаемся к базе
                
                _targetLocalZ = _baseLocalZ;
            }

            // Плавно тянем локальный Z к цели
            var lp = transform.localPosition;
            lp.z = Mathf.SmoothDamp(lp.z, _targetLocalZ, ref _velZ, _smoothTime);
            transform.localPosition = lp;
        }

        private void OnDrawGizmosSelected()
        {
            // Визуализация луча
            Gizmos.color = Color.cyan;
            Vector3 dirWS = transform.TransformDirection(_rayLocalDir.normalized);
            Gizmos.DrawLine(transform.position, transform.position + dirWS * _rayDistance);
        }
    }

}
