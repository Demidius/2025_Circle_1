using System;
using Code.GameEntities.Vehicle;
using Code.TODO;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

namespace Code.GameEntities.Player
{
    public sealed class SmokeHandler : MonoBehaviour
    {
        [Inject] ITankTelemetry _telemetry;

        [SerializeField] VisualEffect _smokeEffect;

        [SerializeField] AnimationCurve _rateByPower  =
            new AnimationCurve(new Keyframe(0f, 0f),  new Keyframe(0.2f, 8f), new Keyframe(1f, 60f));
        [SerializeField] AnimationCurve _speedByPower =
            new AnimationCurve(new Keyframe(0f, 0.5f), new Keyframe(1f, 2f));
        [SerializeField] AnimationCurve _alphaByPower =
            new AnimationCurve(new Keyframe(0f, 0.2f), new Keyframe(1f, 0.8f));

        [SerializeField] float _smooth = 8f;

        float _curRate, _curSpeed, _curAlpha;
        float _prevPower;

        static readonly int RateID       = Shader.PropertyToID("Rate");
        static readonly int StartSpeedID = Shader.PropertyToID("StartSpeed");
        static readonly int AlphaID      = Shader.PropertyToID("Alpha");
        static readonly int PuffID       = Shader.PropertyToID("Puff");
        static readonly int PuffCountID  = Shader.PropertyToID("PuffCount");

        void Start()
        {
            if (_telemetry.EngineOn) _smokeEffect.Play();
            else _smokeEffect.Stop();

            _telemetry.OnEngineOn += StartStopEngine;
        }

        private void OnDestroy()
        {
            _telemetry.OnEngineOn -= StartStopEngine;
        }

        void Update()
        {
            if (_smokeEffect == null) return;

            float power = Mathf.Clamp01(_telemetry.Throttle01);

            float tRate  = _rateByPower.Evaluate(power);
            float tSpeed = _speedByPower.Evaluate(power);
            float tAlpha = _alphaByPower.Evaluate(power);

            float k = 1f - Mathf.Exp(-_smooth * Time.deltaTime);
            _curRate  = Mathf.Lerp(_curRate,  tRate,  k);
            _curSpeed = Mathf.Lerp(_curSpeed, tSpeed, k);
            _curAlpha = Mathf.Lerp(_curAlpha, tAlpha, k);

            _smokeEffect.SetFloat(RateID,       _curRate);
            _smokeEffect.SetFloat(StartSpeedID, _curSpeed);
            _smokeEffect.SetFloat(AlphaID,      _curAlpha);

            float dp = power - _prevPower;
            if (dp > 0.15f && _telemetry.EngineOn)
            {
                float puff = Mathf.RoundToInt(Mathf.Lerp(5, 30, Mathf.Clamp01(dp * 5f)));
                _smokeEffect.SetFloat(PuffCountID, puff);
                _smokeEffect.SendEvent(PuffID);
            }
            _prevPower = power;
        }

        private void StartStopEngine()
        {
            if (_telemetry.EngineOn)
            {
                _smokeEffect.Play();
                _smokeEffect.SendEvent(PuffID);
            }
            else if (!_telemetry.EngineOn)
            {
                _smokeEffect.Stop();
                _smokeEffect.SendEvent(PuffID);
            }
        }
    }
}
