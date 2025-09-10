namespace Code.TODO
{
    public interface ITankTelemetryReadOnly
    {
        bool  EngineOn { get; }
        float Throttle01 { get; } 
        float Turn01 { get; } 
        float SpeedMS { get; } 
        float Speed01 { get; } 
        float AccelMS2 { get; } 
        float Slip01 { get; } 
    }

    public interface ITankTelemetry : ITankTelemetryReadOnly
    {
        void SetEngineOn(bool on);
        void SetThrottle(float throttle01);
        void SetTurn(float turn01);
        void SetKinematics(float speedMS, float speed01, float accelMS2, float slip01);
    }

    public sealed class TankTelemetryService : ITankTelemetry
    {
        public bool  EngineOn   { get; private set; }
        public float Throttle01 { get; private set; }
        public float Turn01     { get; private set; }
        public float SpeedMS    { get; private set; }
        public float Speed01    { get; private set; }
        public float AccelMS2   { get; private set; }
        public float Slip01     { get; private set; }

        public void SetEngineOn(bool on) => EngineOn = on;
        public void SetThrottle(float v) => Throttle01 = UnityEngine.Mathf.Clamp01(v);
        public void SetTurn(float v)     => Turn01 = UnityEngine.Mathf.Clamp(v, -1f, 1f);

        public void SetKinematics(float speedMS, float speed01, float accelMS2, float slip01)
        {
            SpeedMS  = speedMS;
            Speed01  = UnityEngine.Mathf.Clamp01(speed01);
            AccelMS2 = accelMS2;
            Slip01   = UnityEngine.Mathf.Clamp01(slip01);
        }
    }
}
