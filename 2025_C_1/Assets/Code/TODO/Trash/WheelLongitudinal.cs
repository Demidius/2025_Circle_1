using Code.GameEntyties.Player;
using UnityEngine;
using Zenject;

namespace Code.TODO
{
    enum Tracks { LeftTrack, RightTrack }

    public class WheelLongitudinal : MonoBehaviour
    {
        [Inject] private TrackVelocity _trackVelocity;

        [Header("Refs")]
        [SerializeField] private Rigidbody carRb;
        [SerializeField] private Tracks _track;

        [Header("Параметры")]
        [SerializeField] private float topSpeed = 25f;          // м/с (для оценки силы торможения)
        [SerializeField] private float maxDriveForce = 7000f;   // Н на луч
        [SerializeField] private float maxBrakeForce = 10000f;  // Н на луч
        [SerializeField, Range(0,1)] private float traction = 1f;
        [SerializeField, Min(1)] private int wheelsCount = 4;   // сколько лучей делят тягу
        [SerializeField] private float deadZone = 0.05f;        // мёртвая зона для входа гусеницы

        [Header("Ground")]
        [SerializeField] private float rayLength = 1.0f;
        [SerializeField] private LayerMask groundMask = ~0;

        private Transform tire;

        private void Awake() => tire = transform;

        private float GetTrackInput()
        {
            float t = (_track == Tracks.LeftTrack)
                ? _trackVelocity.LeftTrackVelocity
                : _trackVelocity.RightTrackVelocity;

            // ожидаем вход в диапазоне [-1..1]; если у тебя в другом масштабе — нормализуй до этого диапазона
            return Mathf.Clamp(t, -1f, 1f);
        }

        private void FixedUpdate()
        {
            if (!carRb || !tire) return;

            // контакт с землёй
            if (!Physics.Raycast(new Ray(tire.position, -tire.up), out var hit, rayLength, groundMask))
                return;

            // продольное направление по касательной к поверхности
            Vector3 driveDir = Vector3.ProjectOnPlane(tire.forward, hit.normal).normalized;

            // скорость точки колеса вдоль направления тяги
            float vAlong = Vector3.Dot(carRb.GetPointVelocity(tire.position), driveDir);

            // вход с гусеницы (вперёд/назад)
            float input = GetTrackInput();

            float forceScalar;

            if (Mathf.Abs(input) > deadZone)
            {
                // Тяга вперёд/назад по знаку входа
                forceScalar = maxDriveForce * input;
            }
            else
            {
                // Тормоз, когда вход ≈ 0: против направления текущего движения
                if (Mathf.Abs(vAlong) < 0.01f) return; // почти стоим — нечего тормозить

                float speedFactor = Mathf.Clamp01(Mathf.Abs(vAlong) / Mathf.Max(0.01f, topSpeed));
                float brake = maxBrakeForce * speedFactor;

                forceScalar = -Mathf.Sign(vAlong) * brake;
            }

            // учёт сцепления и распределение по количеству лучей
            forceScalar *= traction;
            float perWheel = forceScalar / Mathf.Max(1, wheelsCount);

            // применяем силу
            Vector3 force = driveDir * perWheel;
            carRb.AddForceAtPosition(force, tire.position, ForceMode.Force);
        }
    }
}
