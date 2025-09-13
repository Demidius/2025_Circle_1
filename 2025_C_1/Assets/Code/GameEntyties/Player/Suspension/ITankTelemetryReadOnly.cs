using System;
namespace Code.GameEntities.Vehicle
{
    public interface ITankTelemetryReadOnly
    {
        bool  EngineOn { get; }
        float LeftTrack01  { get; }  
        float RightTrack01 { get; }   
        float Throttle01   { get; }   
        float Turn01       { get; } 
        float SpeedMS      { get; }
        float Speed01      { get; }
        float AccelMS2     { get; }
        float Slip01       { get; }
    }

    public interface ITankTelemetry : ITankTelemetryReadOnly
    {
        void SetEngineOn(bool on);
        void SetTracks(float left01, float right01);
        void SetKinematics(float speedMS, float speed01, float accelMS2, float slip01);
        
        event Action OnEngineOn;
        
        
    }
}
