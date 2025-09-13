using UnityEngine;
using Zenject;

namespace Code.GameEntities.Vehicle
{
    public sealed class TankEngineController : MonoBehaviour
    {
        [Inject] ITankTelemetry _telemetry;
        [Inject] TODO.ITankInputProvider _input;

        void Update()
        {
            if (_input.ToggleEnginePressed)
                _telemetry.SetEngineOn(!_telemetry.EngineOn);

            float left  = Mathf.Clamp(_input.LeftTrackInput,  -1f, 1f);
            float right = Mathf.Clamp(_input.RightTrackInput, -1f, 1f);
            _telemetry.SetTracks(left, right);
        }
    }
}
