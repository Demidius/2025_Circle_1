using Code.TODO;
using UnityEngine;
using Zenject;

namespace Code.GameEntities.Vehicle
{
    public sealed class TankEngineController : MonoBehaviour
    {
        [Inject] ITankTelemetry _telemetry;
        [Inject] ITankInputProvider _input;

        void Update()
        {
            if (_input.ToggleEnginePressed)
                _telemetry.SetEngineOn(!_telemetry.EngineOn);

            float left  = Mathf.Clamp(_input.LeftTrackInput,  -1f, 1f);
            float right = Mathf.Clamp(_input.RightTrackInput, -1f, 1f);

            float throttle01 = Mathf.Clamp01((Mathf.Abs(left) + Mathf.Abs(right)) * 0.5f);
            float turn01     = Mathf.Clamp(right - left, -1f, 1f);

            _telemetry.SetThrottle(throttle01);
            _telemetry.SetTurn(turn01);
        }
    }
}
