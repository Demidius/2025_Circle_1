using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase._2UIModuleF.Buttons
{
    public class ImageUnderCursorEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] Image _underCursorGround;
        [SerializeField] Image _underCursorImage;
        private Color _originalColor;


        private void Start()
        {
            _originalColor = _underCursorImage.color;
            
            DOTween.Sequence()
                .Join(_underCursorGround.DOFade(0f, 0f))
                .Join(_underCursorImage.DOColor(_originalColor, 0f));
        }
        private void OnEnable()
        {
            DOTween.Sequence()
                .Join(_underCursorGround.DOFade(0f, 0f))
                .Join(_underCursorImage.DOColor(_originalColor, 0.2f));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            DOTween.Sequence()
                .Join(_underCursorGround.DOFade(1f, 0.2f))
                .Join(_underCursorImage.DOColor(Color.black, 0.2f));
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            DOTween.Sequence()
                .Join(_underCursorGround.DOFade(0f, 0.2f))
                .Join(_underCursorImage.DOColor(_originalColor, 0.2f));

        }
        public void OnPointerDown(PointerEventData eventData)
        {
            _underCursorImage.DOColor(Color.gray, 0.2f);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            _underCursorImage.DOColor(_originalColor, 0.2f);
        }
    }
}
