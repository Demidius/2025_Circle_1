using System;
using System.Collections;
using System.Threading;
using Code.Common.Async;
using Code.GameEntities.Vehicle;
using CodeBase.System.Core.Consts;
using CodeBase.System.Services.Utilities.Coroutines.CoroutinRuner;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Zenject;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Code.GameEntyties.Player
{
    public class TankAudio : AudioSoursMono
    {
        [Inject] private ITankTelemetry _tanksEngine;
        [Inject] private ITankTelemetryReadOnly _telemetry;
        [Inject] private IReactiveWaiter _waiter;
        [Inject] private ICoroutineRunner _coroutineRunner;

        private EventInstance _engine;
        private EventInstance _engineForce1;
        private EventInstance _engineForce2;
        private EventInstance _trackSound;

        float _prevPower;
        float _powerLerp;
        float _power = 0;


        [SerializeField] private float _paramUpdateLerp = 6f; // скорость сглаживания тяги (в ед/с)
        [SerializeField] private float _gearCooldown = 0.35f; // кулдаун между щелчками передач (сек)
        [SerializeField] private float _forceJerkThreshold = 0.15f; // порог для Start/StopForce по производной тяги
        [SerializeField] private float _hysteresis = 0.03f; // гистерезис для порогов (0.35/0.65)
        [SerializeField] private STOP_MODE _stopMode = STOP_MODE.ALLOWFADEOUT; // как останавливать ивенты

        private CancellationTokenSource _ctsStop;

        private bool _onStarted = false;

        private bool _triggeredLow;
        private bool _triggeredHigh;

        private void OnEnable()
        {
            _tanksEngine.OnEngineOn += OnEngineStartStop;

            _trackSound = RuntimeManager.CreateInstance(_audioTracksBase.TrackSound);
            _trackSound.start();
        }

        private void OnDisable()
        {
            _tanksEngine.OnEngineOn -= OnEngineStartStop;


            _ctsStop?.Cancel();
            _ctsStop?.Dispose();
            _ctsStop = null;

            StopEngine();

            StopInstance(_trackSound);
        }

        private void Update()
        {
            RuntimeManager.StudioSystem.setParameterByName(Const.TankEngineForce, _power);

            // Debug.Log(_power);

            TrackSoundParamsSend();

            float prev = PowerSoundSmooth();

            CheckThresholdCross(prev, _power, 0.35f);
            CheckThresholdCross(prev, _power, 0.65f);

            if (_telemetry.EngineOn)
            {
                UpDownEngineForceSPlay();
            }
        }
        private float PowerSoundSmooth()
        {

            float target = _telemetry.Throttle01;
            float prev = _power;
            _power = Mathf.MoveTowards(_power, target, Time.deltaTime);
            return prev;
        }
        private void TrackSoundParamsSend()
        {
            // Debug.Log(_telemetry.Speed01 + " Speed || \n Slip01 " + _telemetry.Slip01);

            if (_telemetry.Speed01 >= 0.1f)
                RuntimeManager.StudioSystem.setParameterByName(Const.TankSpeed, _telemetry.Speed01);

            // else if (Mathf.Abs(_telemetry.LeftTrack01) >= 0.2f || Mathf.Abs(_telemetry.RightTrack01) >= 0.2f)
            
            if (_telemetry.Slip01 >= 0.1f && _telemetry.EngineOn)
            {
                RuntimeManager.StudioSystem.setParameterByName(Const.TankSpeed, _telemetry.Slip01);
            }
            else  if (_telemetry.Speed01 >= 0.1f)
            {
                RuntimeManager.StudioSystem.setParameterByName(Const.TankSpeed, _telemetry.Speed01);
            }
            else
            {
                RuntimeManager.StudioSystem.setParameterByName(Const.TankSpeed, 0);
            }
        }

        private void UpDownEngineForceSPlay()
        {
            float dp = _telemetry.Throttle01 - _prevPower;
            if (dp > 0.15f)
            {
                _audioManager.PlaySound(_audioTracksBase.StartForceEngine);
            }
            else if (dp < -0.15f)
            {
                _audioManager.PlaySound(_audioTracksBase.StopForceEngine);
            }
            _prevPower = _telemetry.Throttle01;
        }

        void CheckThresholdCross(float prev, float current, float threshold)
        {
            if ((prev < threshold && current >= threshold) ||
                (prev > threshold && current <= threshold))
            {
                _audioManager.PlaySound(_audioTracksBase.TransmissionChanger);
            }
        }

        private void OnEngineStartStop()
        {
            if (_telemetry.EngineOn)
            {
                _engine = RuntimeManager.CreateInstance(_audioTracksBase.Engine);
                _engineForce1 = RuntimeManager.CreateInstance(_audioTracksBase.Force1);
                _engineForce2 = RuntimeManager.CreateInstance(_audioTracksBase.Force2);

                _engine.start();
                _engineForce1.start();
                _engineForce2.start();

                _audioManager.PlaySound(_audioTracksBase.StartEngine);
            }
            else
            {
                // Запускаем отложенную остановку, пока тяга/скорость не упадут
                _ctsStop?.Cancel();
                _ctsStop?.Dispose();
                _ctsStop = new CancellationTokenSource();

                _waiter.WaitBool(
                    () => _power < 0.1f, // условие для глушения
                    true, // ждём пока станет true
                    () => _coroutineRunner.StartCoroutine(StopEngine()), // действие
                    TimeSpan.FromSeconds(0.1), // период опроса
                    _ctsStop.Token // можно отменить, если снова завели
                );
            }
        }



        private IEnumerator StopEngine()
        {

            StopEngineSound();
            Debug.Log("__1__2__");
            yield return new WaitForSeconds(0.5f);
            Debug.Log("___2___");
            StopInstance(_engine);
            StopInstance(_engineForce1);
            StopInstance(_engineForce2);
        }
        private void StopInstance(EventInstance eventInstance)
        {
            if (eventInstance.isValid())
            {
                eventInstance.stop(_stopMode);
                eventInstance.release();
                eventInstance.clearHandle();
            }
        }
        private void StopEngineSound()
        {
            _audioManager.PlaySound(_audioTracksBase.StopEngine);
        }
    }
}
