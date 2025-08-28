using System.Collections;
using CodeBase.System.Core.Consts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace CodeBase._2UIModuleF.Sliders
{
    [RequireComponent(typeof(Slider))]
    public class SliderHoldAction : AudioSoursMono, IPointerDownHandler, IPointerUpHandler
    {
        private bool _isHolding;
        private Coroutine _holdCoroutine;

       
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_holdCoroutine != null)
                StopCoroutine(_holdCoroutine);

            _isHolding = true;
            _holdCoroutine = StartCoroutine(HoldRoutine());
        }
    
        public void OnPointerUp(PointerEventData eventData)
        {
            _isHolding = false;
            if (_holdCoroutine != null)
                StopCoroutine(_holdCoroutine);
        }
        private IEnumerator HoldRoutine()
        {
            while (_isHolding)
            {
                DoSomething();
                yield return new WaitForSeconds(Const.timeHoldRoutine);
            }
        }
       
        private void DoSomething()
        {
            _audioManager.PlaySound(_audioTracksBase.SystemUnderButton, transform.position);
        }
    }

}
