using UnityEngine;
using UnityEngine.VFX;
using Zenject;
namespace Code.GameEntyties.Player
{
    public class SmokeHandler : MonoBehaviour
    {
        [Inject] private TanksEngine _tanksEngine;

        [SerializeField] VisualEffect _smokeEffect;
        [SerializeField] private float _powerSmokeEffect;
        
        private void Start()
        {
            _smokeEffect.Stop();

            _tanksEngine.ChangeEngineState += ChangeSmokeState;
        }

        private void Update()
        {
            if (_smokeEffect) _smokeEffect.SetFloat("Power", _powerSmokeEffect);
        }

        private void ChangeSmokeState(bool state)
        {
            if (state)
                _smokeEffect.Play();
            else
            {
                _smokeEffect.Stop();
            }
        }


    }
}
