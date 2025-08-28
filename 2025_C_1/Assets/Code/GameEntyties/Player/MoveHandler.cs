using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveHandler : MonoBehaviour
{
    [Header("Физика/геометрия")]
    [Tooltip("Расстояние между гусеницами (метры)")]
    public float trackWidth = 2.0f;
    [Tooltip("Макс. линейная скорость корпуса (м/с)")]
    public float maxSpeed = 8f;
    [Tooltip("Макс. угловая скорость (рад/с) при встречных гусеницах")]
    public float maxTurnRateOnSpot = 2.5f;

    [Header("Динамика гусениц")]
    [Tooltip("Ускорение набора тяги (ед/с)")]
    public float accel = 4f;
    [Tooltip("Ускорение торможения (ед/с) — быстрее, чем разгон")]
    public float brakeAccel = 10f;

    [Header("Сопротивление/сцепление")]
    [Tooltip("Демпфирование бокового сноса")]
    public float lateralFriction = 6f;
    [Tooltip("Демпфирование продольного скольжения при отпущенных клавишах")]
    public float longitudinalFriction = 2f;

    private Rigidbody _rb;

    // Текущее «положение газа» каждой гусеницы (-1..+1)
    private float _leftPower;
    private float _rightPower;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.constraints = RigidbodyConstraints.FreezeRotationZ; // для наземной техники (убери если нужно 3D-перевороты)
    }

    void FixedUpdate()
    {
        // ----- Ввод -----
        // Левая гусеница: Q = назад, I = вперёд
        float leftTarget = (Input.GetKey(KeyCode.Q) ? 1f : 0f) + (Input.GetKey(KeyCode.A) ? -1f : 0f);
        leftTarget = Mathf.Clamp(leftTarget, -1f, 1f);

        // Правая гусеница: D = назад, I = вперёд
        float rightTarget = (Input.GetKey(KeyCode.E) ? 1f : 0f) + (Input.GetKey(KeyCode.D) ? -1f : 0f);
        rightTarget = Mathf.Clamp(rightTarget, -1f, 1f);

        // ----- Плавная динамика тяги с усиленным торможением -----
        _leftPower  = MoveWithBrake(_leftPower,  leftTarget,  accel, brakeAccel);
        _rightPower = MoveWithBrake(_rightPower, rightTarget, accel, brakeAccel);

        // ----- Перевод в скорость корпуса (модель дифференциала) -----
        // Линейная скорость вперёд
        float vForward = ((_leftPower + _rightPower) * 0.5f) * maxSpeed;

        // Угловая скорость вокруг вертикальной оси:
        // при чистом «на месте» (лев+1, прав-1) достигнем maxTurnRateOnSpot
        // в общем случае масштабирем по разности гусениц
        float turnFactor = Mathf.Clamp((_rightPower - _leftPower) * 0.5f, -1f, 1f);
        float omega = turnFactor * maxTurnRateOnSpot;

        // Спец-случай: «поворот вокруг стоящей гусеницы»
        // Если одна почти 0, другая > 0: вращаемся радиусом ≈ trackWidth/2 (естественно получается из разности)
        // Это уже покрыто формулой выше. Чтобы стоящая «тормозила», ниже добавим фрикцию.

        // ----- Применение к Rigidbody -----
        Vector3 forward = transform.forward;
        Vector3 desiredVel = forward * vForward;

        // Тушим боковой снос (псевдо-сцепление гусениц)
        Vector3 vel = _rb.linearVelocity;                     // предпочтительно linearVelocity
        Vector3 lateral = Vector3.ProjectOnPlane(vel, forward);
        Vector3 correctedVel = vel - lateral * Mathf.Clamp01(lateralFriction * Time.fixedDeltaTime);

        // Тянем продольную составляющую к desiredVel (мягкое торможение/разгон)
        Vector3 currentForwardVel = Vector3.Project( correctedVel, forward );
        Vector3 newForwardVel = Vector3.MoveTowards(
            currentForwardVel,
            desiredVel,
            longitudinalFriction * Time.fixedDeltaTime * maxSpeed
        );

        // Собираем итоговую скорость
        Vector3 newVel = newForwardVel + (correctedVel - currentForwardVel);
        _rb.linearVelocity = newVel;

        // Устанавливаем угловую скорость (в мировой системе)
        Vector3 angVel = _rb.angularVelocity;
        angVel = new Vector3(0f, omega, 0f);
        _rb.angularVelocity = angVel;
    }

    private static float MoveWithBrake(float current, float target, float accel, float brakeAccel)
    {
        // Если меняем знак или целимся к нулю — используем повышенное тормозное ускорение
        bool braking = Mathf.Sign(current) != Mathf.Sign(target) || Mathf.Approximately(target, 0f);
        float rate = braking ? brakeAccel : accel;
        return Mathf.MoveTowards(current, target, rate * Time.fixedDeltaTime);
    }
}
