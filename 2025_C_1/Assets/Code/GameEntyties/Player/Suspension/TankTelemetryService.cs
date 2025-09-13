using System;
using UnityEngine;

namespace Code.GameEntities.Vehicle
{
    public sealed class TankTelemetryService : ITankTelemetry
    {
        public bool  EngineOn     { get; private set; }
        public float LeftTrack01  { get; private set; }
        public float RightTrack01 { get; private set; }
        public float Throttle01   { get; private set; }
        public float Turn01       { get; private set; }
        public float SpeedMS      { get; private set; }
        public float Speed01      { get; private set; }
        public float AccelMS2     { get; private set; }
        public float Slip01       { get; private set; }

        public event Action OnEngineOn;
        
        public void SetEngineOn(bool on)
        {
            EngineOn = on;
            OnEngineOn?.Invoke();
        }

        public void SetTracks(float left01, float right01)
        {
            LeftTrack01  = Mathf.Clamp(left01,  -1f, 1f);
            RightTrack01 = Mathf.Clamp(right01, -1f, 1f);
            Throttle01   = Mathf.Clamp01((Mathf.Abs(LeftTrack01) + Mathf.Abs(RightTrack01)) * 0.5f);
            Turn01       = Mathf.Clamp(RightTrack01 - LeftTrack01, -1f, 1f);
        }

        public void SetKinematics(float speedMS, float speed01, float accelMS2, float slip01)
        {
            SpeedMS  = speedMS;
            Speed01  = Mathf.Clamp01(speed01);
            AccelMS2 = accelMS2;
            Slip01   = Mathf.Clamp01(slip01);
        }
        
    }
}
