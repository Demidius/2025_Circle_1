using Code.GameEntyties.Player.Suspension;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;
namespace Code.GameEntyties.Player
{
    public class SmokeHandler : MonoBehaviour
    {
        [Inject] private TanksEngine _tanksEngine;
        [Inject] private MoverTank _moveTank;
        

        [SerializeField] VisualEffect _smokeEffect;
     
        [SerializeField] private AnimationCurve _rateByPower  =
            new AnimationCurve(new Keyframe(0f, 0f),  new Keyframe(0.2f, 8f), new Keyframe(1f, 60f)); 
        [SerializeField] private AnimationCurve _speedByPower =
            new AnimationCurve(new Keyframe(0f, 0.5f), new Keyframe(1f, 2f));                        
        [SerializeField] private AnimationCurve _alphaByPower =
            new AnimationCurve(new Keyframe(0f, 0.2f), new Keyframe(1f, 0.8f));                      

        [SerializeField] private float _smooth = 8f;  
        [SerializeField] private float _stopThreshold = 0.05f;
        
        float _curRate, _curSpeed, _curAlpha;
        float _prevPower;

        // Имена свойств из VFX Graph
        static readonly int RateID       = Shader.PropertyToID("Rate");
        static readonly int StartSpeedID = Shader.PropertyToID("StartSpeed");
        static readonly int AlphaID      = Shader.PropertyToID("Alpha");
        static readonly int PuffID       = Shader.PropertyToID("Puff");
        static readonly int PuffCountID  = Shader.PropertyToID("PuffCount");
        
        void OnEnable()  { _tanksEngine.ChangeEngineState += OnEngineState; }
        void OnDisable() { _tanksEngine.ChangeEngineState -= OnEngineState; }




        void Start()
        {
            if (_tanksEngine.IsOn) _smokeEffect.Play();
            else _smokeEffect.Stop();
        }

        private void Update()
        {
            if (_smokeEffect == null) return;
            
            float power = Mathf.Clamp01(Mathf.Abs(_moveTank.EnginePower));
            
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
            
            
            float dp = (power - _prevPower);
            if (dp > 0.15f) {
                int puff = Mathf.RoundToInt(Mathf.Lerp(5, 30, Mathf.Clamp01(dp * 5f)));
                _smokeEffect.SetInt(PuffCountID, puff);
                _smokeEffect.SendEvent(PuffID); 
            }
            _prevPower = power;
        }
        void OnEngineState(bool on)
        {
            if (on) _smokeEffect.Play();
            else _smokeEffect.Stop();
            
            _smokeEffect.SendEvent(PuffID); 
            
        }
    }
}
