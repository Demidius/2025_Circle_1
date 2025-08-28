
using CodeBase.System.Core.Consts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
namespace CodeBase._2UIModuleF.Buttons
{
    public class ScaleOpButton : AudioSoursMono, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private RectTransform _buttonRect;
        private Vector3 _originalScale;
        private Vector3 _baseScale;
        
        public void Play()
        {
            _audioManager.PlaySound(_audioTracksBase.SystemUnderButton, transform.position);
        }
        void Start()
        {
            _buttonRect = GetComponent<RectTransform>();
            if (_buttonRect != null)
            {
                _originalScale = _buttonRect.localScale;
            }
            else
            {
                Debug.LogError("RectTransform не найден на объекте!");
            }
            
            _baseScale = _buttonRect.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_buttonRect != null)
            {
                _buttonRect.DOScale(_originalScale * Const.ButtonHoverScaleMultiplier, Const.ButtonHoverAnimationDuration)
                    .SetEase(Ease.OutQuad);
                Play();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_buttonRect != null)
            {
                _buttonRect.DOScale(_originalScale, Const.ButtonHoverAnimationDuration)
                    .SetEase(Ease.OutQuad);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
           
            if (_buttonRect != null)
            {
                _buttonRect.DOScale(_originalScale * Const.ButtonClickScaleMultiplier, Const.ButtonClickAnimationDuration)
                    .SetEase(Ease.OutQuad)
                    .SetLoops(2, LoopType.Yoyo); 
            }
            _audioManager.PlaySound(_audioTracksBase.BottonDownClick, transform.position);
        }

        // private void OnEnable()
        // {
        //     _buttonRect.localScale = _baseScale; 
        // }

        private void OnDisable()
        {
            if (_buttonRect != null)
            {
                // Останавливаем все активные твины для buttonRect
                _buttonRect.DOKill(); 
                // Устанавливаем масштаб обратно к исходному
                _buttonRect.localScale = _baseScale; 
            }
        }
    }
}