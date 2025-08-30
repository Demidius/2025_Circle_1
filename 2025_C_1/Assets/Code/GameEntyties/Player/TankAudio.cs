using FMOD.Studio;
using FMODUnity;
using Zenject;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Code.GameEntyties.Player
{
    public class TankAudio : AudioSoursMono
    {
        [Inject] private TanksEngine _tanksEngine;

        private EventInstance _engine;

        private void OnEnable()
        {
            _tanksEngine.ChangeEngineState += OnEngineStateChanged;
        }

        private void OnDisable()
        {
            _tanksEngine.ChangeEngineState -= OnEngineStateChanged;


            DG.Tweening.DOVirtual.DelayedCall(1f, () =>
            {
                StopEngine();
            });
        }

        private void OnEngineStateChanged(bool started)
        {
            if (started)
            {
                _engine = RuntimeManager.CreateInstance(_audioTracksBase.Engine);
                _engine.start();
                _audioManager.PlaySound(_audioTracksBase.StartEngine);
            }
            else
            {
                _audioManager.PlaySound(_audioTracksBase.StopEngine);
                StopEngine();
            }
        }

        private void StopEngine()
        {
            if (_engine.isValid())
            {
                _engine.stop(STOP_MODE.IMMEDIATE);
                _engine.release();
                _engine.clearHandle();
            }
        }
    }
}
