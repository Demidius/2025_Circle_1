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

    [Header("Двигатель")]
    [Tooltip("Холостые обороты (RPM)")]
    public float idleRPM = 700f;
    [Tooltip("Максимальные обороты (RPM)")]
    public float maxRPM = 3200f;
    [Tooltip("Скорость реакции оборотов (чем больше, тем резче)")]
    public float rpmResponse = 6f;

    [SerializeField] 
    
    
    // Текущее «положение газа» каждой гусеницы (-1..+1)
    private float _leftPower;
    private float _rightPower;

    // Текущие обороты двигателя (для UI/звука)
    public float engineRPM { get; private set; }

    private Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
        _rb.constraints = RigidbodyConstraints.FreezeRotationZ; // для наземной техники (убери если нужны 3D-перевороты)
        engineRPM = idleRPM;
    }

    void FixedUpdate()
    {
        // ----- Ввод -----
        // Левая гусеница: Q = вперёд, A = назад
        float leftTarget = (Input.GetKey(KeyCode.Q) ? 1f : 0f) + (Input.GetKey(KeyCode.A) ? -1f : 0f);
        leftTarget = Mathf.Clamp(leftTarget, -1f, 1f);

        // Правая гусеница: E = вперёд, D = назад
        float rightTarget = (Input.GetKey(KeyCode.E) ? 1f : 0f) + (Input.GetKey(KeyCode.D) ? -1f : 0f);
        rightTarget = Mathf.Clamp(rightTarget, -1f, 1f);

        // ----- Плавная динамика тяги с усиленным торможением -----
        _leftPower  = MoveWithBrake(_leftPower,  leftTarget,  accel, brakeAccel);
        _rightPower = MoveWithBrake(_rightPower, rightTarget, accel, brakeAccel);

        // ----- Перевод в скорость корпуса (модель дифференциала) -----
        float vForward = ((_leftPower + _rightPower) * 0.5f) * maxSpeed;

        // Разворот: разность гусениц -> угловая скорость
        float turnFactor = Mathf.Clamp((_rightPower - _leftPower) * 0.5f, -1f, 1f);
        float omega = turnFactor * maxTurnRateOnSpot;

        // ----- Применение к Rigidbody -----
        Vector3 forward = transform.forward;
        Vector3 desiredVel = forward * vForward;

        // Тушим боковой снос (имитация сцепления гусениц)
        Vector3 vel = _rb.linearVelocity;
        Vector3 lateral = Vector3.ProjectOnPlane(vel, forward);
        Vector3 correctedVel = vel - lateral * Mathf.Clamp01(lateralFriction * Time.fixedDeltaTime);

        // Тянем продольную составляющую к desiredVel
        Vector3 currentForwardVel = Vector3.Project(correctedVel, forward);
        Vector3 newForwardVel = Vector3.MoveTowards(
            currentForwardVel,
            desiredVel,
            longitudinalFriction * Time.fixedDeltaTime * maxSpeed
        );

        Vector3 newVel = newForwardVel + (correctedVel - currentForwardVel);
        _rb.linearVelocity = newVel;

        // Угловая скорость
        Vector3 angVel = _rb.angularVelocity;
        angVel = new Vector3(0f, omega, 0f);
        _rb.angularVelocity = angVel;

        // ----- Двигатель (RPM) -----
        // «Газ» как среднее по гусеницам (по модулю)
        float throttle = Mathf.Abs((_leftPower + _rightPower) * 0.5f);

        // Добавка RPM при развороте на месте (когда гусеницы встречные)
        // |turnMix| = 0..1, где 1 — чистое вращение на месте
        float turnMix = Mathf.Clamp01(Mathf.Abs(_rightPower - _leftPower) * 0.5f);

        // Целевые обороты: от холостых к максимуму, плюс небольшой бонус при развороте
        float targetRPM = Mathf.Lerp(idleRPM, maxRPM, Mathf.Clamp01(throttle + 0.35f * turnMix));

        // Плавная реакция двигателя
        engineRPM = Mathf.Lerp(engineRPM, targetRPM, rpmResponse * Time.fixedDeltaTime);
    }

    private static float MoveWithBrake(float current, float target, float accel, float brakeAccel)
    {
        // Если меняем знак или целимся к нулю — используем повышенное тормозное ускорение
        bool braking = Mathf.Sign(current) != Mathf.Sign(target) || Mathf.Approximately(target, 0f);
        float rate = braking ? brakeAccel : accel;
        return Mathf.MoveTowards(current, target, rate * Time.fixedDeltaTime);
    }
}
