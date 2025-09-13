using UnityEngine;

namespace Code.GameEntyties.Player
{
    public class TurretCameraAim : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Transform _hull;   // корпус (база локальных осей)
        [SerializeField] private Transform _turret; // башня (локальный поворот по Y)
        [SerializeField] private Transform _gun;    // ствол (локальный поворот по X)
        [SerializeField] private Camera _cam;       // камера игрока
        [SerializeField] private float _anglCorrection = 0f;

        [Header("Скорости (°/с)")]
        [SerializeField] private float _yawSpeedDeg = 240f;
        [SerializeField] private float _pitchSpeedDeg = 180f;

        [Header("Ограничения пушки (°)")]
        [SerializeField] private float _minPitch = -10f;
        [SerializeField] private float _maxPitch = 25f;

        private void Awake()
        {
            if (_cam == null) _cam = Camera.main;
        }

        private void Update()
        {
            if (_cam == null || _hull == null) return;

            Vector3 camFwdWS = _cam.transform.forward;

            // ------------ Башня: локальный yaw относительно корпуса ------------
            if (_turret != null)
            {
                // проекция направления камеры на горизонт корпуса
                Vector3 flatDirWS = Vector3.ProjectOnPlane(camFwdWS, _hull.up);
                if (flatDirWS.sqrMagnitude > 1e-6f)
                {
                    // мировой целевой поворот, выровненный по up корпуса
                    Quaternion targetWorldRot = Quaternion.LookRotation(flatDirWS, _hull.up);
                    // в локаль корпуса
                    Quaternion targetLocalRot = Quaternion.Inverse(_hull.rotation) * targetWorldRot;

                    float currentYaw = NormalizeAngle(_turret.localEulerAngles.y);
                    float targetYaw  = NormalizeAngle(targetLocalRot.eulerAngles.y);

                    float newYaw = Mathf.MoveTowardsAngle(currentYaw, targetYaw, _yawSpeedDeg * Time.deltaTime);

                    Vector3 te = _turret.localEulerAngles;
                    te.x = 0f;
                    te.y = newYaw;   // только локальный Y
                    te.z = 0f;
                    _turret.localEulerAngles = te;
                }
            }

            // ------------ Ствол: локальный pitch относительно корпуса ------------
            if (_gun != null)
            {
                // в локальные координаты башни (после её yaw)
                Vector3 dirTS = _turret.InverseTransformDirection(_cam.transform.forward);

                // угол возвышения: высота к горизонтальной дальности
                float horizLen = new Vector2(dirTS.x, dirTS.z).magnitude;
                float targetPitch = -Mathf.Rad2Deg * Mathf.Atan2(dirTS.y, horizLen);

                // коррекция и кламп
                float minP = _minPitch + _anglCorrection;
                float maxP = _maxPitch + _anglCorrection;
                targetPitch = Mathf.Clamp(targetPitch + _anglCorrection, minP, maxP);

                float currentPitch = NormalizeAngle(_gun.localEulerAngles.x);
                float newPitch = Mathf.MoveTowardsAngle(currentPitch, targetPitch, _pitchSpeedDeg * Time.deltaTime);

                var ge = _gun.localEulerAngles;
                ge.x = newPitch;   // только локальный X
                // не обнуляй Y/Z, если они нужны для отдачи/покачивания;
                // если нет таких эффектов, можно оставить ноль:
                ge.y = 0f;
                ge.z = 0f;
                _gun.localEulerAngles = ge;
            }
        }

        private static float NormalizeAngle(float a) => Mathf.Repeat(a + 180f, 360f) - 180f;
    }
}
